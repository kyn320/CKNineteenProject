using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace SnapToFloor
{
    public class SnapToFloor : MonoBehaviour
    {
        private static MeshFilter[] _meshFiltersToSkip = Array.Empty<MeshFilter>();

        private const int Mesh16BitBufferVertexLimit = 65535;

        private enum ResultType
        {
            X,
            Z
        }

        //스냅이 허용되는 높이
        private const float Height = 1000f;

        //경로를 지정하고, true를 해서 안보이게 하자, %는 윈도우는 컨트롤, 맥은 커맨드 키에 해당된다.
        [MenuItem("Edit/SnapToFloor _END")]
        public static void Snap2Surface()
        {
            var mode = EditorSettings.defaultBehaviorMode;

            //Selection은 현재 에디터에서 선택된 오브젝트를 뜻한다.
            foreach (Transform transform in Selection.transforms)
            {
                Undo.RecordObject(transform, "SnapUndoAction");
                if (mode == EditorBehaviorMode.Mode2D)
                {
                    SpriteRenderer[] hasSpriteRenders = transform.GetComponentsInChildren<SpriteRenderer>();
                    bool hasChildSpriteRender = false;
                    Collider2D hasCollider = transform.GetComponent<Collider2D>();

                    //자식이 SpriteRender를 가지고 있는지 체크합니다.
                    foreach (var spriteRenderer in hasSpriteRenders)
                    {
                        if (spriteRenderer != null)
                        {
                            hasChildSpriteRender = true;
                            goto FindSpriteRender;
                        }
                    }

                    FindSpriteRender:
                    //경고 메세지
                    if (!hasChildSpriteRender && hasCollider == null)
                    {
                        Debug.LogError("Could not find sprite renderer and collider 2D");
                        return;
                    }
                    else if (hasCollider == null)
                    {
                        Debug.LogError("Could not find collider 2D");
                        return;
                    }
                    else if (!hasChildSpriteRender)
                    {
                        Debug.LogError("Could not find sprite renderer");
                        return;
                    }

                    #region 매쉬의 버텍스에 대한 월드 계산 위치

                    Vector3 minMaxDistance = GetMinMaxRangeByVertex2D(transform);

                    float footYPosition = GetMinYVertex2D(transform);

                    #endregion

                    float distance = minMaxDistance.z;

                    float startPosition = minMaxDistance.x;

                    //간격에 따른 알맞는 간격을 계산한다.
                    float intervalValue = CalculateSeparationByDistance(distance);

                    //간격에 알맞는 알갱이를 가져옴
                    int numberOfGrains = CalculatePointCount(distance, intervalValue);
                    int nowNumberOfGrains = numberOfGrains + 1;

                    Vector3 position = transform.position;
                    float? moveY = null;

                    //원하는 알갱이 수 만큼 반복한다
                    //- 원래는 내가 원하는 간격을 제시하면 그것에 맞는 알갱이를 뿌린다.
                    for (int i = 0; i < nowNumberOfGrains; i++)
                    {
                        //그려낼 위치에서 사이간격에 맞춰 그려냄
                        float xx = startPosition + intervalValue * i;

                        Vector3 drawPosition = new Vector3(xx, footYPosition, position.z);

                        //각각의 오브젝트의 위치에서 아래 방향으로 Ray를 쏜다.
                        RaycastHit2D[] hits = Physics2D.RaycastAll(drawPosition, Vector2.down, Height);

                        //각각 hit정보 확인
                        foreach (var hit in hits)
                        {
                            //자기 자신의 콜라이더를 맞춘 경우 pass : 예외 처리
                            if (hit.collider.gameObject == transform.gameObject)
                                continue;

                            if (moveY == null)
                                moveY = hit.distance;
                            else
                            {
                                if (moveY > hit.distance)
                                    moveY = hit.distance;
                            }
                        }
                    }

                    position.y -= moveY ?? 0f;
                    //hit된 위치로 이동시킨다.
                    transform.position = position;
                }

                if (mode == EditorBehaviorMode.Mode3D)
                {
                    //자식 오브젝트가 있는 자식의 매쉬 필터를 가져온다.
                    SkinnedMeshRenderer[] skinnedMesh = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

                    int childCount = transform.childCount;

                    //매쉬 필터를 가져온다.
                    MeshFilter mfSelf = transform.GetComponent<MeshFilter>();

                    //자식을 가지고 있는지 체크
                    bool hasChild = childCount > 0;
                    bool hasSkinnedMesh = skinnedMesh.Length > 0;
                    bool hasMfSelf = mfSelf;
                    Transform tr = transform;

                    if (hasMfSelf && childCount > 0)
                    {
                        GameObject go = new GameObject();
                        go.transform.position = transform.position;
                        transform.SetParent(go.transform);

                        tr = go.transform;
                    }

                    //자식이 있는 경우 컴바인한다.
                    if (!hasSkinnedMesh && hasChild)
                    {
                        // 우리가 부모-자식 계층을 끊고 부모 계층을 다시 얻을 때 때로는 scale이 약간 달라지므로 끊기전에 scale을 저장한다.
                        Vector3 oldScaleAsChild = tr.localScale;

                        // 부모 오브젝트 계층안에 있으면 트랜스폼에 영향이 가므로, 부모 계층을 잠시 끊는다.
                        int positionInParentHierarchy = tr.GetSiblingIndex();
                        Transform parent = tr.parent;
                        tr.parent = null;

                        // 덕분에 새로 결합된 메시는 자식과 같은 세계 공간에서 동일한 위치와 크기를 갖게 됩니다.:
                        Quaternion oldRotation = tr.rotation;
                        Vector3 oldPosition = tr.position;
                        Vector3 oldScale = tr.localScale;
                        tr.rotation = Quaternion.identity;
                        tr.position = Vector3.zero;
                        tr.localScale = Vector3.one;

                        //기존에 매쉬 필터가 없으면 해당 오브젝트에 추가하고,
                        //이미 있으면 새로운 오브젝트를 만들어서 거기에 추가한다.
                        mfSelf = tr.gameObject.AddComponent<MeshFilter>();

                        //컴바인 시스템 동작
                        CombineSystem(tr);

                        // 변환 값을 다시 가져옵니다.:
                        tr.rotation = oldRotation;
                        tr.position = oldPosition;
                        tr.localScale = oldScale;

                        // 상위 및 동일한 계층 위치를 다시 가져옵니다.:
                        tr.parent = parent;
                        tr.SetSiblingIndex(positionInParentHierarchy);

                        //스케일 값을 자식으로 다시 설정:
                        tr.localScale = oldScaleAsChild;
                    }
                    else
                    {
                        //스키니드가 아니고, 자식 오브젝트가 없는 경우
                        if (!hasSkinnedMesh)
                        {
                            if (Application.systemLanguage == SystemLanguage.Korean)
                                Assert.IsNotNull(mfSelf, "매쉬 필터가 없습니다.");
                            else
                                Assert.IsNotNull(mfSelf, "Can't find Mesh filter");
                        }
                    }

                    #region 매쉬의 버텍스에 대한 월드 계산 위치

                    Vector2 minMaxByX = GetMinMaxRangeByVertex3D(ResultType.X, tr, mfSelf);
                    Vector3 vx1 = Vector3.zero;
                    vx1.x = minMaxByX.x;

                    Vector3 vx2 = Vector3.zero;
                    vx2.x = minMaxByX.y;

                    Vector2 minMaxByZ = GetMinMaxRangeByVertex3D(ResultType.Z, tr, mfSelf);
                    Vector3 vz1 = Vector3.zero;
                    vz1.x = minMaxByZ.x;

                    Vector3 vz2 = Vector3.zero;
                    vz2.x = minMaxByZ.y;

                    float footYPosition = GetMinYVertex3D(tr);

                    #endregion

                    float distanceX = Vector3.Distance(vx1, vx2);
                    float distanceZ = Vector3.Distance(vz1, vz2);

                    float startPositionX = minMaxByX.x;
                    float startPositionZ = minMaxByZ.x;

                    //간격에 따른 알맞는 간격을 계산한다.
                    float intervalValueX = CalculateSeparationByDistance(distanceX);
                    float intervalValueZ = CalculateSeparationByDistance(distanceZ);

                    //간격에 알맞는 알갱이를 가져옴
                    int numberOfGrainsX = CalculatePointCount(distanceX, intervalValueX);
                    int numberOfGrainsZ = CalculatePointCount(distanceZ, intervalValueZ);
                    int nowNumberOfGrainsX = numberOfGrainsX + 1;
                    int nowNumberOfGrainsZ = numberOfGrainsZ + 1;

                    Vector3 position = tr.position;
                    float? moveY = null;

                    //원하는 알갱이 수 만큼 반복한다
                    //- 원래는 내가 원하는 간격을 제시하면 그것에 맞는 알갱이를 뿌린다.
                    for (int i = 0; i < nowNumberOfGrainsX; i++)
                    {
                        for (int j = 0; j < nowNumberOfGrainsZ; j++)
                        {
                            //그려낼 위치에서 사이간격에 맞춰 그려냄
                            float xx = startPositionX + intervalValueX * i;
                            float zz = startPositionZ + intervalValueZ * j;

                            Vector3 drawPosition = new Vector3(xx, footYPosition, zz);

                            //각각의 오브젝트의 위치에서 아래 방향으로 Ray를 쏜다.
                            RaycastHit[] hits = Physics.RaycastAll(drawPosition, Vector3.down, Height);

                            //각각 hit정보 확인
                            foreach (RaycastHit hit in hits)
                            {
                                //자기 자신의 콜라이더를 맞춘 경우 pass : 예외 처리
                                if (hit.collider.gameObject == tr.gameObject)
                                    continue;

                                if (moveY == null)
                                    moveY = hit.distance;
                                else
                                {
                                    if (moveY > hit.distance)
                                        moveY = hit.distance;
                                }
                            }
                        }
                    }

                    position.y -= moveY ?? 0f;
                    //hit된 위치로 이동시킨다.
                    tr.position = position;

                    switch (hasMfSelf)
                    {
                        case true when childCount > 0:
                            transform.parent = null;
                            DestroyImmediate(tr.gameObject);
                            break;
                        case false:
                            DestroyImmediate(mfSelf);
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// x,y는 min,max를 반환하고, z는 두 점의 거리를 반환합니다.
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        private static Vector3 GetMinMaxRangeByVertex2D(Transform tr)
        {
            Collider2D boxCollider2D = tr.GetComponent<Collider2D>();

            Bounds bounds = boxCollider2D.bounds;
            float y = bounds.min.y;

            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            max.y = y;

            float distance = Vector2.Distance(min, max);

            return new Vector3(min.x, max.x, distance);
        }

        /// <summary>
        /// 바운딩 박스의 minY를 반환합니다.
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        private static float GetMinYVertex2D(Transform tr)
        {
            Collider2D boxCollider2D = tr.GetComponent<Collider2D>();

            Bounds bounds = boxCollider2D.bounds;

            float worldY = bounds.min.y;

            return worldY;
        }

        private static Vector2 GetMinMaxRangeByVertex3D(ResultType resultType, Transform tr, MeshFilter mf)
        {
            //스키니드 매쉬 렌더러를 가진 경우 발 밑에서 쏘도록 처리합니다.
            if (!mf)
            {
                Vector3 position = tr.position;
                return resultType switch
                {
                    ResultType.X => new Vector2(position.x, position.x),
                    ResultType.Z => new Vector2(position.z, position.z),
                    _ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null)
                };
            }

            Mesh mesh = mf.sharedMesh;

            //Default로 버텍스 0을 넣어봅니다.
            //로컬좌표에 있는 Vertical 0을 월드좌표로 변환합니다.
            float min;
            float max;

            //초기화
            switch (resultType)
            {
                case ResultType.X:
                    min = tr.TransformPoint(mesh.vertices[0]).x;
                    max = tr.TransformPoint(mesh.vertices[0]).x;
                    break;
                case ResultType.Z:
                    min = tr.TransformPoint(mesh.vertices[0]).z;
                    max = tr.TransformPoint(mesh.vertices[0]).z;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null);
            }

            foreach (Vector3 point in mesh.vertices)
            {
                //로컬좌표에 있는 Vertical 0을 월드좌표로 변환합니다.
                Vector3 worldPoint = tr.TransformPoint(point);

                switch (resultType)
                {
                    case ResultType.X:
                    {
                        if (min > worldPoint.x)
                            min = worldPoint.x;

                        if (max < worldPoint.x)
                            max = worldPoint.x;
                        break;
                    }
                    case ResultType.Z:
                    {
                        if (min > worldPoint.z)
                            min = worldPoint.z;

                        if (max < worldPoint.z)
                            max = worldPoint.z;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null);
                }
            }

            return new Vector2(min, max);
        }

        private static float GetMinYVertex3D(Transform tr)
        {
            MeshFilter meshFilter = tr.GetComponent<MeshFilter>();

            //매쉬렌더러가 없으면 객체 피봇 위치를 반환
            if (meshFilter == null)
                return tr.position.y;

            Mesh mesh = meshFilter.sharedMesh;

            //Default로 버텍스0을 넣어줍니다.
            //로컬좌표에 있는 매쉬 버텍스를 월드좌표로 변환합니다.
            Vector3 minY = tr.TransformPoint(mesh.vertices[0]);

            foreach (Vector3 point in mesh.vertices)
            {
                //로컬좌표에 있는 버텍스을 월드좌표로 변환합니다.
                Vector3 worldPoint = tr.TransformPoint(point);

                if (minY.y > worldPoint.y)
                    minY.y = worldPoint.y;
            }

            return minY.y;
        }

        private static MeshFilter[] GetMeshFiltersToCombine(Transform transform)
        {
            //이 GameObject와 그 자식에 속한 모든 MeshFilter 가져오기:
            MeshFilter[] meshFilters = transform.GetComponentsInChildren<MeshFilter>(true);

            //meshFiltersToSkip 배열에서 이 GameObject에 속한 첫 번째 MeshFilter 삭제:
            _meshFiltersToSkip = _meshFiltersToSkip.Where((meshFilter) => meshFilter != meshFilters[0]).ToArray();

            //meshFiltersToSkip 배열에서 null 값 삭제:
            _meshFiltersToSkip = _meshFiltersToSkip.Where((meshFilter) => meshFilter != null).ToArray();

            return _meshFiltersToSkip.Aggregate(meshFilters,
                (current, t) => current.Where((meshFilter) => meshFilter != t).ToArray());
        }

        private static void CombineSystem(Transform transform)
        {
            // 이 GameObject와 그 자식에 속한 모든 MeshFilter 가져오기:
            MeshFilter[] meshFilters = GetMeshFiltersToCombine(transform);

            //스키니드 매쉬렌더러를 쓰면 스키니드를 따라가고, 그 외는 매쉬렌더러를 따라간다.
            CombineInstance[]
                combineInstances =
                    new CombineInstance[meshFilters.Length - 1]; //첫 번째 MeshFilter는 이 GameObject에 속하므로 필요하지 않습니다.:

            // 65535 이상이면 32 비트 인덱스 버퍼를 사용한다.
            long verticesLength = 0;

            // 이 루프에서 이 GameObject에 속하는 첫 번째 MeshFilter 건너뛰기.
            for (int i = 0; i < meshFilters.Length - 1; i++)
            {
                combineInstances[i].subMeshIndex = 0;
                combineInstances[i].mesh = meshFilters[i + 1].sharedMesh;
                combineInstances[i].transform = meshFilters[i + 1].transform.localToWorldMatrix;
                verticesLength += combineInstances[i].mesh.vertices.Length;
            }

            // CombineInstances에서 메시 생성:
            Mesh combinedMesh = new Mesh();

            //버텍스가 범위를 넘을 경우 32비트로 처리한다.
            if (verticesLength > Mesh16BitBufferVertexLimit)
            {
                combinedMesh.indexFormat =
                    UnityEngine.Rendering.IndexFormat.UInt32; // Unity 2017.3 이상에서만 작동합니다.
            }

            combinedMesh.CombineMeshes(combineInstances);
            meshFilters[0].sharedMesh = combinedMesh;
        }

        private static float CalculateSeparationByDistance(float distance)
        {
            float numberOfGrain;
            int sampling = 3; //기본 샘플링
            do
            {
                sampling++;
                numberOfGrain = distance / sampling;
            } while (numberOfGrain > 0.2f);

            return numberOfGrain;
        }

        private static int CalculatePointCount(float distance, float intervalValue)
        {
            int result = (int) (distance / intervalValue);
            if (result <= 0)
                result = 0;
            return result;
        }
    }
}