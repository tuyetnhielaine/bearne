--hướng dẫn
--https://www.youtube.com/watch?v=yc6TUkO_SVg&t=310s
--2009
function tbEquip:Tip_RandAttrib(nState, tbEnhRandMASS)	-- 获得Tip字符串：随机属性

	local szTip = "";
	local nPos1, nPos2 = KItem.GetEquipActive(KItem.EquipType2EquipPos(it.nDetail));
	local tbMASS = it.GetRandMASS();			-- 获得道具随机魔法属性

	if (nState == Item.TIPS_PREVIEW) or (nState == Item.TIPS_GOODS) then	-- 属性预览状态，显示魔法属性范围

		local nSeries = it.nSeries;
		local tbGenInfo = it.GetGenInfo(0, 1);
		
		if (nState == Item.TIPS_PREVIEW) then	-- 预览状态需读取配置表中的五行
			local tbBaseProp = KItem.GetEquipBaseProp(it.nGenre, it.nDetail, it.nParticular, it.nLevel, it.nVersion);
			if (tbBaseProp) then
				nSeries = tbBaseProp.nSeries;
			else
				nSeries = -1;					-- 五行不确定
			end
		end

		if (not nPos1) or (not nPos2) then		-- 不参与五行激活的装备

			for _, tbMA in ipairs(tbGenInfo) do
				local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion);
				if tbMAInfo then
					szTip = szTip.."\n"..self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));		
				end
			end

		else									-- 参与五行激活的装备

			for i = 1, #tbGenInfo / 2 do		-- 明属性处理
				local tbMA = tbGenInfo[i];
				local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion);
				if tbMAInfo then
					szTip = szTip.."\n"..self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
				end
			end

			local nTotal  = 0;					-- 暗属性总计数
			local nActive = 0;					-- 已激活暗属性计数
			for i = #tbGenInfo / 2 + 1, #tbGenInfo do	-- 暗属性处理
				local tbMA = tbGenInfo[i];
				if tbMA.szName ~= "" then
					nTotal = nTotal + 1;
					if tbMA.bActive == 1 then
						nActive = nActive + 1;
					end
				else
					break;
				end
			end

			if nTotal > 0 then					-- 存在暗属性

				if nSeries < 0 then				-- 五行不确定的情况

					szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (0/%d)", nTotal);
					szTip = szTip..string.format(
						"<color=gray>%s (?) %s (?) nhân vật (?)<color>",
						Item.EQUIPPOS_NAME[nPos1],
						Item.EQUIPPOS_NAME[nPos2]
					);		-- 总是灰的
					szTip = szTip.."<color>";

					for i = #tbGenInfo / 2 + 1, #tbGenInfo do
						local tbMA = tbGenInfo[i];
						local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion);
						if tbMAInfo then
							local szDesc = self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
							if (szDesc ~= "") and (tbMASS[i].bVisible == 1) then
								szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);	-- 总是灰的
							end
						end
					end

				else

					local nAccSeries  = KMath.AccruedSeries(it.nSeries);
					local szAccSeries = Env.SERIES_NAME[nAccSeries];
					local pEquip1 = me.GetEquip(nPos1);
					local pEquip2 = me.GetEquip(nPos2);
					local nSeries1 = pEquip1 and pEquip1.nSeries or Env.SERIES_NONE;
					local nSeries2 = pEquip2 and pEquip2.nSeries or Env.SERIES_NONE;

					szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (%d/%d)\n", nActive, nTotal);

					if (nSeries1 ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
					else
						szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
					end
					if (nSeries2 ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
					else
						szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
					end
					if (me.nSeries ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>Nhân vật (%s)<color>", szAccSeries);
					else
						szTip = szTip..string.format("<color=white>Nhân vật (%s)<color>", szAccSeries);
					end

					szTip = szTip.."<color>";

					for i = #tbGenInfo / 2 + 1, #tbGenInfo do
						local tbMA = tbGenInfo[i];
						local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion);
						if tbMAInfo then
							local szDesc = self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
							if (szDesc ~= "") and (tbMASS[i].bVisible == 1) then
								szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);	-- 总是灰的
							end
						end
					end

				end

			end

		end

	else										-- 其他状态，显示魔法属性具体值

		--add by Tuyết Nhi.
		local tbGenInfo = it.GetGenInfo(0, 1);	--khai báo bổ sung MIN-MAX.

		if (not nPos1) or (not nPos2) then		-- 不参与五行激活的装备

			for i = 1, #tbMASS do

				local tbMA = tbMASS[i];
				local szDesc = "";
				if tbEnhRandMASS then
					szDesc = self:GetMagicAttribDescEx2(tbMA.szName, tbMA.tbValue, tbEnhRandMASS[i].tbValue);
				else
					szDesc = self:GetMagicAttribDesc(tbMA.szName, tbMA.tbValue);
				end
				if (szDesc ~= "") and (tbMA.bVisible == 1) then
					if (tbMA.bActive ~= 1) then
						szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);
					else
						szTip = szTip.."\n"..szDesc;
					end
--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion);
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
--------------------------------------------------
				end

			end

		else												-- 参与五行激活的装备

			for i = 1, #tbMASS / 2 do			-- 明属性处理
				local tbMA = tbMASS[i];
				local szDesc = "";
				if tbEnhRandMASS then
					szDesc = self:GetMagicAttribDescEx2(tbMA.szName, tbMA.tbValue, tbEnhRandMASS[i].tbValue);
				else
					szDesc = self:GetMagicAttribDesc(tbMA.szName, tbMA.tbValue);
				end
				if (szDesc ~= "") and (tbMA.bVisible == 1) then
					if (tbMA.bActive ~= 1) then
						szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);
					else
						szTip = szTip.."\n"..szDesc;
					end
--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion);
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
--------------------------------------------------
				end

			end

			local nTotal  = 0;					-- 暗属性总计数
			local nActive = 0;					-- 已激活暗属性计数
			for i = #tbMASS / 2 + 1, #tbMASS do	-- 暗属性处理
				local tbMA = tbMASS[i];
				if tbMA.szName ~= "" then
					nTotal = nTotal + 1;
					if tbMA.bActive == 1 then
						nActive = nActive + 1;
					end
				else
					break;
				end
			end

			if nTotal > 0 then					-- 存在暗属性
				local nAccSeries  = KMath.AccruedSeries(it.nSeries);
				local szAccSeries = Env.SERIES_NAME[nAccSeries];
				local pEquip1 = me.GetEquip(nPos1);
				local pEquip2 = me.GetEquip(nPos2);
				local nSeries1 = pEquip1 and pEquip1.nSeries or Env.SERIES_NONE;
				local nSeries2 = pEquip2 and pEquip2.nSeries or Env.SERIES_NONE;
				szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (%d/%d)\n", nActive, nTotal);
				if (nSeries1 ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
				else
					szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
				end
				if (nSeries2 ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
				else
					szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
				end
				if (me.nSeries ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>Nhân vật (%s)<color>", szAccSeries);
				else
					szTip = szTip..string.format("<color=white>Nhân vật (%s)<color>", szAccSeries);
				end
				szTip = szTip.."<color>";
				for i = #tbMASS / 2 + 1, #tbMASS do
					local tbMA = tbMASS[i];
					local szDesc = "";
					if tbEnhRandMASS then
						szDesc = self:GetMagicAttribDescEx2(tbMA.szName, tbMA.tbValue, tbEnhRandMASS[i].tbValue);
					else
						szDesc = self:GetMagicAttribDesc(tbMA.szName, tbMA.tbValue);
					end
					if (szDesc ~= "") and (tbMA.bVisible == 1) then
						if tbMA.bActive == 1 then
							szTip = szTip..string.format("\n%s", szDesc);
						else
							szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);
						end
--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion);
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
--------------------------------------------------
					end
				end
			end

		end

	end
	if szTip ~= "" then
		return	"\n<color=greenyellow>"..szTip.."<color>";
	end
	return szTip;
end
----------------------------------------------------------------------



--linux 13 môn phái
function tbEquip:Tip_RandAttrib(nState, tbEnhRandMASS)	-- 获得Tip字符串：随机属性

	local nOpen = KGblTask.SCGetDbTaskInt(DBTASK_ENHANCESIXTEEN_OPEN);
	local szTip = "";
	local nPos1, nPos2 = KItem.GetEquipActive(KItem.EquipType2EquipPos(it.nDetail));
	local tbMASS = it.GetRandMASS();			-- 获得道具随机魔法属性

	if (nState == Item.TIPS_PREVIEW) or (nState == Item.TIPS_GOODS) then	-- 属性预览状态，显示魔法属性范围

		local nSeries = it.nSeries;
		local tbGenInfo = it.GetGenInfo(0, 1);

		if (nState == Item.TIPS_PREVIEW) then	-- 预览状态需读取配置表中的五行
			local tbBaseProp = KItem.GetEquipBaseProp(it.nGenre, it.nDetail, it.nParticular, it.nLevel, it.nVersion);
			if (tbBaseProp) then
				nSeries = tbBaseProp.nSeries;
			else
				nSeries = -1;					-- 五行不确定
			end
		end

		if (not nPos1) or (not nPos2) then		-- 不参与五行激活的装备

			for _, tbMA in ipairs(tbGenInfo) do
				local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion, it.nMAVersion);
				if tbMAInfo then
					szTip = szTip.."\n"..self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo.tbRange));
				end
			end

		else									-- 参与五行激活的装备

			for i = 1, #tbGenInfo / 2 do		-- 明属性处理
				local tbMA = tbGenInfo[i];
				local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion, it.nMAVersion);
				if tbMAInfo then
					szTip = szTip.."\n"..self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
				end
			end

			local nTotal  = 0;					-- 暗属性总计数
			local nActive = 0;					-- 已激活暗属性计数
			for i = #tbGenInfo / 2 + 1, #tbGenInfo do	-- 暗属性处理
				local tbMA = tbGenInfo[i];
				if tbMA.szName ~= "" then
					nTotal = nTotal + 1;
					if tbMA.bActive == 1 then
						nActive = nActive + 1;
					end
				else
					break;
				end
			end

			if nTotal > 0 then					-- 存在暗属性

				if nSeries < 0 then				-- 五行不确定的情况

					szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (0/%d)", nTotal);
					szTip = szTip..string.format(
						"<color=gray>%s (?) %s (?) nhân vật (?)<color>",
						Item.EQUIPPOS_NAME[nPos1],
						Item.EQUIPPOS_NAME[nPos2]
					);		-- 总是灰的
					szTip = szTip.."<color>";

					for i = #tbGenInfo / 2 + 1, #tbGenInfo do
						local tbMA = tbGenInfo[i];
						local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion, it.nMAVersion);
						if tbMAInfo then
							local szDesc = self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
							if (szDesc ~= "") and (tbMASS[i].bVisible == 1) then
								szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);	-- 总是灰的
							end
						end
					end

				else

					local nAccSeries  = KMath.AccruedSeries(it.nSeries);
					local szAccSeries = Env.SERIES_NAME[nAccSeries];
					local pEquip1 = me.GetEquip(nPos1);
					local pEquip2 = me.GetEquip(nPos2);
					local nSeries1 = pEquip1 and pEquip1.nSeries or Env.SERIES_NONE;
					local nSeries2 = pEquip2 and pEquip2.nSeries or Env.SERIES_NONE;

					szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (%d/%d)\n", nActive, nTotal);

					if (nSeries1 ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
					else
						szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
					end
					if (nSeries2 ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
					else
						szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
					end
					if (me.nSeries ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>Nhân vật (%s)<color>", szAccSeries);
					else
						szTip = szTip..string.format("<color=white>Nhân vật (%s)<color>", szAccSeries);
					end

					szTip = szTip.."<color>";

					for i = #tbGenInfo / 2 + 1, #tbGenInfo do
						local tbMA = tbGenInfo[i];
						local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion, it.nMAVersion);
						if tbMAInfo then
							local szDesc = self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
							if (szDesc ~= "") and (tbMASS[i].bVisible == 1) then
								szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);	-- 总是灰的
							end
						end
					end

				end

			end

		end

	else										-- 其他状态，显示魔法属性具体值
		
		--add by Tuyết Nhi.
		local tbGenInfo = it.GetGenInfo(0, 1);		--khai báo bổ sung MIN-MAX.
		if (not nPos1) or (not nPos2) then		-- 不参与五行激活的装备

			for i = 1, #tbMASS do
				local tbMA = tbMASS[i];
				local szDesc = "";
				if tbEnhRandMASS and (nOpen == 0 or (nOpen == 1 and it.nEnhTimes < Item.nEnhTimesLimitOpen - 1)) then
					szDesc = self:GetMagicAttribDescEx2(tbMA.szName, tbMA.tbValue, tbEnhRandMASS[i].tbValue);
				else
					szDesc = self:GetMagicAttribDesc(tbMA.szName, tbMA.tbValue);
				end
				if (szDesc ~= "") and (tbMA.bVisible == 1) then
					if (tbMA.bActive ~= 1) then
						szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);
					else
						szTip = szTip.."\n"..szDesc;
					end
--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion, it.nMAVersion); -- LINUX có thêm it.nMAVersion
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
--------------------------------------------------
				end
			end

		else
												-- 不参与五行激活的装备
			for i = 1, #tbMASS / 2 do			-- 明属性处理
				local tbMA = tbMASS[i];
				local szDesc = "";				
				if tbEnhRandMASS and (nOpen == 0 or (nOpen == 1 and it.nEnhTimes < Item.nEnhTimesLimitOpen - 1)) then
					szDesc = self:GetMagicAttribDescEx2(tbMA.szName, tbMA.tbValue, tbEnhRandMASS[i].tbValue);
				else
					szDesc = self:GetMagicAttribDesc(tbMA.szName, tbMA.tbValue);
				end
				if (szDesc ~= "") and (tbMA.bVisible == 1) then
					if (tbMA.bActive ~= 1) then
						szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);
					else
						szTip = szTip.."\n"..szDesc;
					end
--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion, it.nMAVersion); -- LINUX có thêm it.nMAVersion
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
--------------------------------------------------
				end
			end

			local nTotal  = 0;					-- 暗属性总计数
			local nActive = 0;					-- 已激活暗属性计数
			for i = #tbMASS / 2 + 1, #tbMASS do	-- 暗属性处理
				local tbMA = tbMASS[i];
				if tbMA.szName ~= "" then
					nTotal = nTotal + 1;
					if tbMA.bActive == 1 then
						nActive = nActive + 1;
					end
				else
					break;
				end
			end

			if nTotal > 0 then					-- 存在暗属性
				local nAccSeries  = KMath.AccruedSeries(it.nSeries);
				local szAccSeries = Env.SERIES_NAME[nAccSeries];
				local pEquip1 = me.GetEquip(nPos1);
				local pEquip2 = me.GetEquip(nPos2);
				local nSeries1 = pEquip1 and pEquip1.nSeries or Env.SERIES_NONE;
				local nSeries2 = pEquip2 and pEquip2.nSeries or Env.SERIES_NONE;
				szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (%d/%d)\n", nActive, nTotal);
				if (nSeries1 ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
				else
					szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
				end
				if (nSeries2 ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
				else
					szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
				end
				if (me.nSeries ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>Nhân vật (%s)<color>", szAccSeries);
				else
					szTip = szTip..string.format("<color=white>Nhân vật (%s)<color>", szAccSeries);
				end
				szTip = szTip.."<color>";
				for i = #tbMASS / 2 + 1, #tbMASS do
					local tbMA = tbMASS[i];
					local szDesc = "";
					if tbEnhRandMASS and (nOpen == 0 or (nOpen == 1 and it.nEnhTimes < Item.nEnhTimesLimitOpen - 1)) then
						szDesc = self:GetMagicAttribDescEx2(tbMA.szName, tbMA.tbValue, tbEnhRandMASS[i].tbValue);
					else
						szDesc = self:GetMagicAttribDesc(tbMA.szName, tbMA.tbValue);
					end
					if (szDesc ~= "") and (tbMA.bVisible == 1) then
						if tbMA.bActive == 1 then
							szTip = szTip..string.format("\n%s", szDesc);
						else
							szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);
						end
--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion, it.nMAVersion); -- LINUX có thêm it.nMAVersion
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
--------------------------------------------------
					end
				end
			end

		end

	end
	if szTip ~= "" then
		return	"<color=greenyellow>"..szTip.."<color>";
	end
	return szTip;
end



















--2014
function tbEquip:Tip_RandAttrib(nState, tbEnhRandMASS)	-- 获得Tip字符串：随机属性

	local nOpen = KGblTask.SCGetDbTaskInt(DBTASK_ENHANCESIXTEEN_OPEN);
	local szTip = "";
	local nPos1, nPos2 = KItem.GetEquipActive(KItem.EquipType2EquipPos(it.nDetail));
	local tbMASS = it.GetRandMASS();			-- 获得道具随机魔法属性
	local tbPercent = self:GetRandPercent();  --tỷ lệ % Quyển Trục TL bản 2014.

	if (nState == Item.TIPS_PREVIEW) or (nState == Item.TIPS_GOODS) then	-- 属性预览状态，显示魔法属性范围

		local nSeries = it.nSeries;
		local tbGenInfo = it.GetGenInfo(0, 1);

		if (nState == Item.TIPS_PREVIEW) then	-- 预览状态需读取配置表中的五行
			local tbBaseProp = KItem.GetEquipBaseProp(it.nGenre, it.nDetail, it.nParticular, it.nLevel, it.nVersion);
			if (tbBaseProp) then
				nSeries = tbBaseProp.nSeries;
			else
				nSeries = -1;					-- 五行不确定
			end
		end

		if (not nPos1) or (not nPos2) then		-- 不参与五行激活的装备

			for _, tbMA in ipairs(tbGenInfo) do
				local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion, it.nMAVersion);
				if tbMAInfo then
					szTip = szTip.."\n"..self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo.tbRange));
				end
			end

		else									-- 参与五行激活的装备

			for i = 1, #tbGenInfo / 2 do		-- 明属性处理
				local tbMA = tbGenInfo[i];
				local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion, it.nMAVersion);
				if tbMAInfo then
					szTip = szTip.."\n"..self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
				end
			end

			local nTotal  = 0;					-- 暗属性总计数
			local nActive = 0;					-- 已激活暗属性计数
			for i = #tbGenInfo / 2 + 1, #tbGenInfo do	-- 暗属性处理
				local tbMA = tbGenInfo[i];
				if tbMA.szName ~= "" then
					nTotal = nTotal + 1;
					if tbMA.bActive == 1 then
						nActive = nActive + 1;
					end
				else
					break;
				end
			end

			if nTotal > 0 then					-- 存在暗属性

				if nSeries < 0 then				-- 五行不确定的情况

					szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (0/%d)", nTotal);
					szTip = szTip..string.format(
						"<color=gray>%s (?) %s (?) nhân vật (?)<color>",
						Item.EQUIPPOS_NAME[nPos1],
						Item.EQUIPPOS_NAME[nPos2]
					);		-- 总是灰的
					szTip = szTip.."<color>";

					for i = #tbGenInfo / 2 + 1, #tbGenInfo do
						local tbMA = tbGenInfo[i];
						local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion, it.nMAVersion);
						if tbMAInfo then
							local szDesc = self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
							if (szDesc ~= "") and (tbMASS[i].bVisible == 1) then
								szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);	-- 总是灰的
							end
						end
					end

				else

					local nAccSeries  = KMath.AccruedSeries(it.nSeries);
					local szAccSeries = Env.SERIES_NAME[nAccSeries];
					local pEquip1 = me.GetEquip(nPos1);
					local pEquip2 = me.GetEquip(nPos2);
					local nSeries1 = pEquip1 and pEquip1.nSeries or Env.SERIES_NONE;
					local nSeries2 = pEquip2 and pEquip2.nSeries or Env.SERIES_NONE;

					szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (%d/%d)\n", nActive, nTotal);

					if (nSeries1 ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
					else
						szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
					end
					if (nSeries2 ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
					else
						szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
					end
					if (me.nSeries ~= nAccSeries) then
						szTip = szTip..string.format("<color=gray>Nhân vật (%s)<color>", szAccSeries);
					else
						szTip = szTip..string.format("<color=white>Nhân vật (%s)<color>", szAccSeries);
					end

					szTip = szTip.."<color>";

					for i = #tbGenInfo / 2 + 1, #tbGenInfo do
						local tbMA = tbGenInfo[i];
						local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbMA.nLevel, it.nVersion, it.nMAVersion);
						if tbMAInfo then
							local szDesc = self:GetMagicAttribDescEx(tbMA.szName, self:BuildMARange(tbMAInfo));
							if (szDesc ~= "") and (tbMASS[i].bVisible == 1) then
								szTip = szTip..string.format("\n<color=gray>%s<color>", szDesc);	-- 总是灰的
							end
						end
					end

				end

			end

		end

	else										-- 其他状态，显示魔法属性具体值
		--add by Tuyết Nhi.
		local tbGenInfo = it.GetGenInfo(0, 1);		--khai báo bổ sung MIN-MAX.
		if (not nPos1) or (not nPos2) then		-- 不参与五行激活的装备

			for i = 1, #tbMASS do
				local tbMA = tbMASS[i];
				local tbEnhMA = tbEnhRandMASS and tbEnhRandMASS[i] or nil
				local szDesc = self:GetRandAttribDesc(nState, nOpen, tbPercent[i], tbMA, tbEnhMA);
				if (szDesc ~= "") and (tbMA.bVisible == 1) then
					if (tbMA.bActive ~= 1) then
						szTip = szTip..string.format("<color=gray>%s<color>", szDesc);
					else
						szTip = szTip..""..szDesc;
					end
					--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion, it.nMAVersion); -- LINUX có thêm it.nMAVersion
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: 1 DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
					--------------------------------------------------
				end
			end

		else
												-- 参与五行激活的装备
			for i = 1, #tbMASS / 2 do			-- 明属性处理
				local tbMA = tbMASS[i];
				local tbEnhMA = tbEnhRandMASS and tbEnhRandMASS[i] or nil
				local szDesc = self:GetRandAttribDesc(nState, nOpen, tbPercent[i], tbMA, tbEnhMA);
				if (szDesc ~= "") and (tbMA.bVisible == 1) then
					if (tbMA.bActive ~= 1) then
						szTip = szTip..string.format("<color=gray>%s<color>", szDesc);
					else
						szTip = szTip..""..szDesc;
					end
					--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion, it.nMAVersion); -- LINUX có thêm it.nMAVersion
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: 2 DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
					--------------------------------------------------
				end
			end

			local nTotal  = 0;					-- 暗属性总计数
			local nActive = 0;					-- 已激活暗属性计数
			for i = #tbMASS / 2 + 1, #tbMASS do	-- 暗属性处理
				local tbMA = tbMASS[i];
				if tbMA.szName ~= "" then
					nTotal = nTotal + 1;
					if tbMA.bActive == 1 then
						nActive = nActive + 1;
					end
				else
					break;
				end
			end

			if nTotal > 0 then					-- 存在暗属性
				local nAccSeries  = KMath.AccruedSeries(it.nSeries);
				local szAccSeries = Env.SERIES_NAME[nAccSeries];
				local pEquip1 = me.GetEquip(nPos1);
				local pEquip2 = me.GetEquip(nPos2);
				local nSeries1 = pEquip1 and pEquip1.nSeries or Env.SERIES_NONE;
				local nSeries2 = pEquip2 and pEquip2.nSeries or Env.SERIES_NONE;
				szTip = szTip..string.format("\n<color=blue>Kích hoạt NH (%d/%d)\n", nActive, nTotal);
				if (nSeries1 ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
				else
					szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos1], szAccSeries);
				end
				if (nSeries2 ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
				else
					szTip = szTip..string.format("<color=white>%s (%s)<color> ", Item.EQUIPPOS_NAME[nPos2], szAccSeries);
				end
				if (me.nSeries ~= nAccSeries) then
					szTip = szTip..string.format("<color=gray>Nhân vật (%s)<color>", szAccSeries);
				else
					szTip = szTip..string.format("<color=white>Nhân vật (%s)<color>", szAccSeries);
				end
				szTip = szTip.."<color>";
				for i = #tbMASS / 2 + 1, #tbMASS do
					local tbMA = tbMASS[i];
					local tbEnhMA = tbEnhRandMASS and tbEnhRandMASS[i] or nil
					local szDesc = self:GetRandAttribDesc(nState, nOpen, tbPercent[i], tbMA, tbEnhMA);
					if (szDesc ~= "") and (tbMA.bVisible == 1) then
						if tbMA.bActive == 1 then
							szTip = szTip..string.format("%s", szDesc);
						else
							szTip = szTip..string.format("<color=gray>%s<color>", szDesc);
						end
						--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion, it.nMAVersion); -- LINUX có thêm it.nMAVersion
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: 3 DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
						--------------------------------------------------
					end
				end
			end

		end

	end
	if szTip ~= "" then
		return	"<color=greenyellow>"..szTip.."<color>";
	end
	return szTip;
end


















--add by Tuyết Nhi.
function tbEquip:GetValueOptMinMaxNe(tbMin, tbMax, tbValue)
	if not tbMin or not tbMax or not tbValue or not tbMin[1] or not tbMax[1] or not tbValue[1] then
		me.Msg("[Lỗi GetValueOptMinMaxNe] xin liên hệ GM, thank you.");
		return "[Lỗi GetValueOptMinMaxNe]";	--is nil value
	end
	
	--DEBUG.
	--me.Msg(string.format("GetValueOptMinMaxNe: MIN = %d, MAX = %d, CUR = %d.", tbMin[1], tbMax[1], tbValue[1]));
	--Vì thuộc tính tăng thêm do cường hóa nên chỉ hiển thị min-max thay vì MAX ban đầu.
	--EX: thuộc tính ban đầu 80/100 nhưng cường hóa tăng lên 115/100 thì vượt giá trị MAX ban đầu, nên sẽ hiển thị là min-max.
    local curValue = (tbValue and tbValue[1]) or 0;
    if curValue == tbMax[1] then
        return " <color=salmon>[MAX]<color>";
    else
        return string.format(" <color=white>[%d-%d]<color>", tbMin[1], tbMax[1]);
    end
end







--------------------------------------------------
--add by Tuyết Nhi, thêm hiển thị MIN-MAX
local tbMAInfo = KItem.GetRandAttribInfo(tbMA.szName, tbGenInfo[i]["nLevel"], it.nVersion, it.nMAVersion); -- LINUX có thêm it.nMAVersion
if tbMAInfo and tbMA.tbValue then
    local tbMin, tbMax = self:BuildMARange(tbMAInfo);
    szTip = szTip ..self:GetValueOptMinMaxNe(tbMin, tbMax, tbMA.tbValue);
    me.Msg("Tip_RandAttrib: DEBUG " ..tbMA.szName.. "-> cấp " ..tbGenInfo[i]["nLevel"]..".");
end
--------------------------------------------------
