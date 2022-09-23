--������ : �̻��
--��� ȯ�� : Max 2008 �̻� (nodeLocalBoundingBox �� 2008 �̻�)

global RefBones		-- �Ѿƿ����� �۷ι� ������ �ν��ϱ� ���� �ʱ⿡ �� �� ���

try (destroydialog RefBones) catch()		-- Ȥ�� �����ִ� â�� ������ ���� ����

rollout RefBones "Reference Bones Tool v0.27" width:200
(
	local IDKEYWORD = "RefBone"		-- Ref Bone ���� �˾ƺ��� ���������� ������ Ű���� ������
	local IDKEYWORDHANDLE = "RefBoneHandle"		-- Ref Bone ���� �˾ƺ��� ���������� ������ Ű���� ������
	local IDKEYWORDTARGETHANDLE = "RefBoneTargetHandle"		-- Ref �� �� Ÿ�ٿ� Ref ���� �ڵ� ��ȣ�� ���
	local NAMEPREFIX = "Ref_"
	
	button uiBtnProcess "Process" width:70 across:2
	button uiBtnSelect "Select" width:70
	spinner uiSpnBoneWidth "BoneScale%" offset:[30, 0] tooltip:"�����Ǵ� ���� ũ��, RefBones�� ���� �� �������ּ���." range:[1, 500, 100] type:#integer align:#right width:110 across:2
	colorPicker uiColorWireColor color:yellow offset:[30, -2] toolTip:"RefBones�� ���� �� ���� �������ּ���."

	--checkbox <name> [<caption>] [checked:<boolean>] [triState:<integer>][tooltip:<string>
	checkbox uiChkOptPos "Position" checked:true across:3
	checkbox uiChkOptRot "Rotation" checked:true
	checkbox uiChkOptScale "Scale" checked:true
	
	checkbox uiChkOptInheritScale "Inherit Scale" checked:false align:#left toolTip:"Default: Off\rƯ���� ��찡 �ƴϸ� Inherit Scale�� ���⸦ �����մϴ�."
	
	/*
	Set Scalable Biped ��ư�� ô�߿� ����� ��ġ�� �������ִ� ����ִԱ��� �߰��ϴ� �۾��� �ʿ��ؼ� ����� ���� Ȧ�� --> ����ִ� ��ġ ������ �غ��� ���µ�, �ܼ� �����Ͽ��� ��߳��� �������� ��Ģ�� ����. (�������� Ű�� �ɾ���� ���� Ȯ����)
	button uiBtnSetScalableBiped "Set Scalable Biped" tooltip:"Scale�� ������ �����е�� �����ϰ� Scale�� �����ϴ� ���۸� �߰��մϴ�."
	*/
	
	button uiBtnHelp "About"
	
	function GetAlignBB obj = (		-- ���� ��ǥ�󿡼� �����Ӱ� ȸ���Ǿ��ִ� ������Ʈ�� �ٿ�� �ڽ� �������� ���� �������� ��ġ�� ȸ���� �����Ͽ� point �� �� ���� �����Ѵ�. �ַ� ���̸� ��� ���� �뵵
		bb = nodeGetBoundingBox obj obj.transform		-- ���� �ٿ�� �ڽ� ������ �� �� ���� �迭�� bb�� ����� bb[1], bb[2]
		
		if ((classof obj.baseobject) == Point) do
		(	-- ����Ʈ���۴� �����θ� �����ؼ� �ٿ���ڽ��� ������ �־�����Ѵ�.
			bb[1].x -= obj.baseobject.size * 0.5
			bb[1].y -= obj.baseobject.size * 0.5
			bb[2].x += obj.baseobject.size * 0.5
			bb[2].y += obj.baseobject.size * 0.5
		)
		return bb
	)
	
	function GetWidthX obj = (		-- ���õ� ������Ʈ�� ���� �ٿ�� �ڽ� �������� x�� ������ �β��� �˾Ƴ���. (���̰� �ƴ� �β� --> ���� �β� ���� ����ϱ� ����)
		bb = GetAlignBB obj		-- ���� �������� ȸ���� ��ġ�� ���µ� �ٿ�� �ڽ� ����
		return (((abs (bb[2].y - bb[1].y)) + (abs (bb[2].z - bb[1].z))) / 2.0)		-- z�� �β��� y�� �β��� ��հ��� ���Ѵ�.
	)
	
	function CheckWorkingBone obj = (		-- � �۾��� ���� �˻�. "IGNORE", "CREATE", "MODIFY" �̷��� �� ���� string�� �����Ѵ�.
		if (getUserProp obj "RefBone") == true do (return "MODIFY")
		
		tFlag = false		-- �ӽ� �÷��� ����
		
		-------------------- �˻� ����
		if (classof obj.baseobject == Biped_Object) do (tFlag = true)		-- �����е��� ���, ������̾ ����� �����е峪 ������ baseobject ó���������
		if (obj.BoneEnable == true) do (tFlag = true)		-- ���� ���
		if (classof obj.baseobject == Point) do (tFlag = true)
		if (classof obj.baseobject == Dummy) do (tFlag = true)
		
		if ((getUserProp obj IDKEYWORD) == true) do (tFlag = false)		-- ���������� RefBone �̸� ������ false ó��
		-------------------- �˻� ��
		
		if tFlag == true then (return "CREATE") else (return "IGNORE")
	)

	-- children�� Array�� �ٷ� ��ȯ�� �ȵż� ���� �Լ�. �迭�� ����
	function ChildrenToArray childrenArray = (
		local returnArray = #()
		for o in childrenArray do (
			append returnArray o
		)
		return returnArray
	)
	
	function FindLookAtPos obj = (		-- �����е��� ��ó�� �ڽ��� ���� �ִ� ���� �Ӹ��� �ٶ󺸸鼭 RefBone �� �����Ǿ�� �Ѵ�. �̷� ��� �ȶ��ϰ� �ٶ� ���� ã���ִ� �Լ�. �ٶ� ��ġ�� �����Ѵ�.
	-- �۵� �����, �θ� �� ��ġ���� �ڽĺ� ��ġ�� �Ÿ��� ��� �˾Ƴ���, �θ����κ��� x������ �̵����״��� �Ÿ��� ������ �־����� �ٶ󺸴� ���� �ƴ϶�� ������� ����
	-- �˻����� obj �� �Ϲ� ���� ��� .length ���� Ȱ���Ѵ�.
	-- �ڽ��� ���� �����е� ������Ʈ�� �Ϲ� ������Ʈ�� BoneOn �� ��쿡�� �ٿ�� �ڽ� ������ ���̷� Ȱ���Ѵ�.
		
		tMatrix = obj.transform		-- �������� ���µ� �������� tLength ����� �ؾ���
		tMatrix.scale = [1, 1, 1]
		tMatrix.rotation = obj.transform.rotation
		tMatrix.position = obj.transform.position
		
		tLength = 1.0		-- ����� ���� ���� ����. ����Ʈ���� 1.0
		
		if obj.children.count == 0 then (		-- �ڽ��� �ϳ��� ���� ��� ���� length ���� �̿��ϰų� ���� �ٿ�� �ڽ� �߽����κ��� ���� x�� ���̸� �̿��Ѵ�.
			tLength = case (classof obj.baseobject) of
			(
			BoneGeometry: obj.baseobject.length * obj.transform.scale.x
			Biped_Object: (biped.getTransform obj #scale).x
			default: (GetAlignBB obj)[2].x
			)
			/*
			if ((classof obj.baseobject) == BoneGeometry) then (
				tLength = obj.baseobject.length * obj.transform.scale.x
			)
			else (
				tLength = (GetAlignBB obj)[2].x
			)
			*/
		)
		else		-- �ڽ��� �ϳ� �̻��� ���
		(
			local targetNode
			local beforeDist = -1.0
            local childrenObjs = #() -- �ڽĵ��� ������ �� �����е尡 �ϳ��� ������ �����е常 ������� �ڽ��� �ٽ� �����ϱ� ���� �迭
            
            for o in obj.children do
            (
                if (classof o.baseobject) == Biped_Object do
                (
                    append childrenObjs o -- �����е�� �Ǹ��Ǹ� �迭�� �߰�
                )

            )
            
            if childrenObjs.count == 0 do
            (
                -- �����е尡 �ϳ��� �߰ߵ��� ������ childrenObjs�� ���� �ڽĵ��� �ٽ� ����
                childrenObjs = ChildrenToArray obj.children
            )

			for o in childrenObjs do
			(
				vDist = distance obj.transform.pos o.transform.pos		-- ���� ������Ʈ�κ��� �ڽ� ������Ʈ�� �Ÿ��� ����
				if vDist > 0.001 do		-- ���� ��ġ�� �ִ� ���� ó�� ��󿡼� �����Ѵ�. ���� ��� �ڽ��� �ϳ��� ���� �� �ִµ� �̶��� tLength �� �⺻���� 1.0�� �ȴ�.
				(
					xPos = ((transMatrix [vDist, 0, 0]) * tMatrix).pos		-- �ڽĿ�����Ʈ �Ÿ���ŭ ���� ������Ʈ�� x������ �̵��� ��ġ
					endDist = distance xPos o.transform.pos		-- �̵��� ��ġ�κ��� �ڽ� ������Ʈ������ �Ÿ� (�� �Ÿ��� ª�� ���� ���� ��ǥ �ڽ���. x�࿡ ���ĵǾ����� ���� �� �Ÿ��� ª��. x�࿡ ����� �� �Ÿ��� Ŀ����.)
					
					if beforeDist == -1.0 OR endDist <= beforeDist do		-- ���� o�� endDist �� �������� �� ª���� o�� Ÿ�� ���� ���� �� beforeDist ������Ʈ
					(
						targetNode = o
						beforeDist = endDist
						tLength = (cos ((90 - acos ((endDist * 0.5) / vDist)) * 2)) * vDist		-- Ÿ���� x�� �߽ɿ��� ��߳����� ��쿡�� �ﰢ�Լ��� ����ؾ���
					)
				)
			)
		)
		
		return ((transMatrix [tLength, 0, 0]) * tMatrix).pos		-- obj�κ��� tLength ��ŭ �̵��� Ʈ�������� pos ����
	)
	
	function CreateBone pStart pEnd vUp vSize = (		-- ������, ����, ������, �β� �Է¹ް� �� ����. ������ ��� �� ������Ʈ�� �������� ����Ʈ�� ���� ����
		tBone = BoneSys.createBone pStart pEnd vUp
		tBone.width = vSize
		tBone.height = vSize
		tBone.taper = 90.0
		-- tBone.length �̰��� �ڵ� ������
		tBone.sidefins = false
		tBone.sidefinssize = vSize * 0.5
		tBone.sidefinsstarttaper 
		tBone.sidefinsendtaper 
		tBone.frontfin = false
		tBone.frontfinsize = vSize * 0.5
		tBone.frontfinstarttaper 
		tBone.frontfinendtaper 
		tBone.backfin = false
		tBone.backfinsize = vSize * 0.5
		tBone.backfinstarttaper = 10.0
		tBone.backfinendtaper = 10.0
		tBone.genmap = false
		
		tBone.boneScaleType=#none
		tBone.boneAutoAlign=false
		
		return tBone
	)
	
	function SetRefBoneAttr targetObj newObj = (		-- Ref Bone �Ӽ��� �÷��� �����Ѵ�
		newObj.WireColor = uiColorWireColor.color
		setUserProp newObj IDKEYWORD "true"
		setUserProp newObj IDKEYWORDHANDLE targetObj.inode.handle
		setUserProp targetObj IDKEYWORDTARGETHANDLE newObj.inode.handle
		newObj.name = NAMEPREFIX + targetObj.name
	)
	
	function SetRefController targetObj newObj = (		-- Ÿ�� ������Ʈ�� �����ϵ��� ��ũ��Ʈ ��Ʈ�ѷ��� ����. ���� ��� ���� ������� transform ������ ����.
		-- �ϴ� ��Ʈ�ѷ� ����
		newObj.pos.controller = Position_XYZ ()
		newObj.rotation.controller = Euler_XYZ ()
		newObj.scale.controller = bezier_scale ()

		newObj.scale = targetObj.transform.scale
		newObj.rotation = targetObj.transform.rotation
		newObj.pos = targetObj.transform.pos
		
		if (uiChkOptPos.state == true) do (
			newObj.pos.controller = Position_Constraint ()
			newObj.pos.controller.appendTarget targetObj 50.0
		)

		if (uiChkOptRot.state == true) do (
			newObj.rotation.controller = Orientation_Constraint ()
			newObj.rotation.controller.appendTarget targetObj 50.0
		)

		if (uiChkOptScale.state == true) do (
			newObj.scale.controller = scale_script ()
			newObj.scale.controller.AddNode "TG" targetObj
			newObj.scale.controller.SetExpression "tScale = try (TG.transform.scale * TG.inode.stretchTM.scale) catch ([1, 1, 1])
if abs (tScale.x - 1.0) < 0.00001 do (tScale.x = 1.0)
if abs (tScale.y - 1.0) < 0.00001 do (tScale.y = 1.0)
if abs (tScale.z - 1.0) < 0.00001 do (tScale.z = 1.0)
tScale"
		)

		-- Scale ���
		if ( uiChkOptInheritScale.state == false ) then (
			setInheritanceFlags newObj #{1, 2, 3, 4, 5, 6}		-- scale ��� ���� ������ ���� ���� inherit �� ����
		)
		else (
			setInheritanceFlags newObj #{1, 2, 3, 4, 5, 6, 7, 8, 9}		-- scale �� ������ ����Ϸ��� ����
		)
	)

	-- �������� �����ϴ� ���� (�����е� ��)�� �����Ѵ�.
	function GetRefOrigin ref = (
		local handle = getUserProp ref IDKEYWORDHANDLE
		return (maxOps.getNodeByHandle handle)
	)

	-- ������ �����ϴ� Ref���� �����Ѵ�. ������ Ref�� ����� ���ų� ã�� �� ������ undefined ����.
	function GetRef origin = (
		local refBone
		try (refBone = maxOps.getNodeByHandle (getUserProp origin IDKEYWORDTARGETHANDLE)) catch (
			refBone = undefined
		)

		return refBone
	)
	
	function CheckIfExist obj = (		-- ���� ó���Ϸ��� �ϴ� ������Ʈ�� �̹� Ref Bone �� �����Ǿ��ִ����� ����. Ref Bone ���� ������ ������ ������ IDKEYWORDHANDLE = "RefBoneHandle" �� ���� ������Ƽ�� ���� ���� �ڵ� ��ȣ�� �ֱ⶧���� �̸� ������)
		if obj == undefined do return undefined		-- �θ��� Ref Bone �� ã�� �������� undefined �� obj �� �Էµ� �� �־ ����ڵ�
		
		local rNode
		rNode = GetRef obj -- �ϴ� ���� ������Ʈ�� Ref�� ����� �ִ��� ã�ƺ���.
		if (rNode != undefined) do (return rNode) -- ����� ������ �� ������Ʈ�� ����

		-- ����� ������ ��ü ������Ʈ �ȿ��� �� ã�ƺ���.
		for o in objects do (
			if (obj.inode.handle == getUserProp o IDKEYWORDHANDLE) do (rNode = o)
		)
		return rNode		-- Ref Bone �� �����Ѵ�. ��ã���� undefined ����
	)
	
	function AutoParent targetObj newObj = (		-- ���� ������ Ref Bone �� �θ� �ڵ� �����ϵ��� �ϴ� �Լ�
		if targetObj.parent == undefined do return ()		-- Ÿ�� ������Ʈ�� �θ� ������ �׳� �����ؾ���

		tObj = CheckIfExist targetObj.parent		-- Ÿ�� ������Ʈ �θ��� Ref Bone �� ã�´�.
		if tObj != undefined do (newObj.parent = tObj)		-- Ÿ�� �θ��� Ref Bone�� ã������ ���� ���� Ref Bone�� �θ�� ��ũ
	)
	
	-- obj �� ��� �ڽĵ��� �迭�� ����. �迭 ������ �������� �������
	function GetAllChildren obj = (
		if ( obj == undefined ) do return undefined
		
		local tAllChildren = #()
		if ( obj.children.count != 0 ) do (
			for o in obj.children do (
				append tAllChildren o
				if ( o.children.count != 0 ) do (
					tAllChildren = tAllChildren +  (GetAllChildren o)		-- recursive
				)
			)
		)
		
		return tAllChildren
	)
	
	-- ������ �����е���, �Ҽӵ� ��� �����е带 ����.
	function GetAllBiped obj = (
		if ( obj == undefined ) do return undefined
		if ( classof obj.baseobject != Biped_Object ) do return undefined
		
		local bipedRoot = obj.controller.rootNode
		
		local allChildren = GetAllChildren bipedRoot
		
		local allBiped = #()
		for o in allChildren do
		(
			if classof o.baseobject == Biped_Object do
			(
				append allBiped o
			)
		)
		return allBiped
	)

	on uiBtnProcess pressed do (
		if selection.count == 0 do return ()		-- ���õ� ������Ʈ�� ������ �׳� ����
		
		undo on (		-- Undo �� �����ϵ���
			local tTarget = #()		-- �θ� ��ũ �۾��� ���ؼ��� ��� Ref Bone �� ���ܳ� �ڿ� for ���� �� �� �� �������ϹǷ� �۾� ����� ����Ű�� �迭�� �ʿ���
			local tNew = #()			-- �θ� ��ũ �۾��� ���ؼ��� ��� Ref Bone �� ���ܳ� �ڿ� for ���� �� �� �� �������ϹǷ� �۾� ����� ����Ű�� �迭�� �ʿ���
			for o in selection do (
				checkStr = CheckWorkingBone o
				case of (
					(checkStr == "CREATE"):(
						-- �����е带 ������ ä Process�� ������. Ref���� �����ϰų� ���� Ref���� ������.
						local tReply = #yes		-- ������ ����� ����ϴ� ���� ����
						local foundRefBone = CheckIfExist o
						
						local tBone
						if foundRefBone != undefined then (
							tBone = foundRefBone
						)
						else (
							tBone = CreateBone o.transform.pos (FindLookAtPos o) o.dir ((GetWidthX o) * (uiSpnBoneWidth.value * 0.01))
						)
						
						SetRefBoneAttr o tBone		-- RefBone �Ӽ� ����
						SetRefController o tBone		-- ��Ʈ�ѷ� ����
						if ((classof o.baseobject) != Point AND (classof o.baseobject) != Dummy) do
						(
							o.boxMode = on
						)
						append tTarget o
						append tNew tBone
					)
					(checkStr == "MODIFY"):(
						-- Ref���� ������ ä Process�� ������. Ref���� �����ϱ⸸ ��'
						-- �� ��� o�� Ref��
						local tOrigin = GetRefOrigin o

						SetRefBoneAttr tOrigin o		-- RefBone �Ӽ� ����
						SetRefController tOrigin o		-- ��Ʈ�ѷ� ����

						if ((classof tOrigin.baseobject) != Point AND (classof tOrigin.baseobject) != Dummy) do (
							tOrigin.boxMode = on
						)
						append tTarget tOrigin
						append tNew o
					)
					default:()
				)
			) -- for end
			if tTarget.count != 0 do (		-- CheckWorkingBone üũ�� ���� tTarget ������ 0�� ���ɼ��� ����.
				--for o = 1 to tTarget.count do (AutoParent tTarget[o] tNew[o])
			)
			
			select tNew
		)
	)
	
	on uiBtnSelect pressed do (
		tSel = #()
		for o in objects do (
			if ((getUserProp o IDKEYWORD) == true) do (append tSel o)
		)
		select tSel
	)
	
	on uiSpnBoneWidth changed var do (
		if selection.count == 0 do return ()
		tObj = #()
		for o in selection do (
			if (getUserProp o IDKEYWORD) == true do (append tObj o)		-- ���� ������Ʈ�� �ȿ��� Ref Bone �� ��󳽴�
		)
		if tObj.count == 0 do return ()
		
		for o in tObj do (
			tNode = maxOps.getNodeByHandle (getUserProp o IDKEYWORDHANDLE)		-- �ڵ� ��ȣ�� Ÿ�� ���� ������ �˾Ƴ���. ��ã���� tNode �� undefined
			tScale = 1.0 / ((tNode.transform.scale.y + tNode.transform.scale.z) * 0.5)
			if tNode != undefined do (
				tValue = (GetWidthX tNode) * (uiSpnBoneWidth.value * 0.01 * tScale)
				o.width = tValue
				o.height = tValue
			)
		)			
	)
	
	on uiColorWireColor changed var do (
		for o in selection do (
			if ((getUserProp o IDKEYWORD) == true) do (o.wirecolor = var)
		)
	)
	
	-- �����Ͻ� 1�� ô�߿� ����� ��ġ�� ������ ���� �ְԵȴ�. �̰��� �������ִ� Position ����ִ� ��Ʈ�ѷ��� �߰��ؾ���.
	on uiBtnSetScalableBiped pressed do
	(
		if selection[1] == undefined do return()
		if (classof selection[1].baseobject != Biped_Object) do return()
		
	
		local allBiped = GetAllBiped selection[1]
		local rootBiped = selection[1].controller.rootNode
		local pelvis = biped.getNode selection[1] #Pelvis link:1
		
		local newPoint = Point isSelected:off Box:on Centermarker:off axistripod:off cross:off size:((GetWidthX pelvis) * 2.2) wirecolor:green
		newPoint.transform = rootBiped.transform
		newPoint.parent = rootBiped
		
		-- Ȥ�� �� ������ �����ϱ� ���� figure���� ���ش�.
		rootBiped.controller.figureMode = false
		
		--paramWire.connect $.transform.controller[#Scale] $'Bip001 L Forearm'.transform.controller.Biped_SubAnim.controller.BipScaleList.controller[#Bezier_Scale] "Scale"

		for o in allBiped do
		(
			if ( classof o.controller == BipSlave_Control) do
			(
				o.controller.Biped_SubAnim.controller.BipScaleList.Available.controller = bezier_scale()
				o.controller.Biped_SubAnim.controller.BipScaleList.active = o.controller.Biped_SubAnim.controller.BipScaleList.count
				
				paramWire.connect newPoint.transform.controller[#Scale] o.transform.controller.Biped_SubAnim.controller.BipScaleList.controller[o.controller.Biped_SubAnim.controller.BipScaleList.count] "Scale"
			)
		)
		select newPoint
	)

	on uiBtnHelp pressed do (
		shellLaunch "http://cafe.naver.com/pinksox/5035" ""
	)
	
	on RefBones open do (

	)
	
	on RefBones close do (

	)
)

createDialog RefBones style:#(#style_titlebar, #style_toolwindow, #style_sysmenu) lockWidth:true