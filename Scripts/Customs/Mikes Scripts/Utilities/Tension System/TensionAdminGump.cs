using System;
using Server;
using Server.Gumps;
using Server.Network;
using System.Globalization; // Needed for CultureInfo.InvariantCulture

namespace Bittiez.CustomSystems
{
    public class TensionAdminGump : Gump
    {
        private const int ApplyTensionButtonID = 1;
        private const int TensionTextEntryID = 0;

        private const int ApplyReductionButtonID = 2;
        private const int ReductionTextEntryID = 1;

        // *** 新增：生物死亡张力增量控件ID ***
        private const int ApplyCreatureDeathIncrementButtonID = 3;
        private const int CreatureDeathIncrementTextEntryID = 2;

        private const int LABEL_X = 50;
        private const int TEXT_ENTRY_X = 200; // 稍微调整X坐标以便对齐
        private const int APPLY_BUTTON_X_AFTER_TEXT_ENTRY = 290; // 稍微调整X坐标以便对齐

        public TensionAdminGump() : base(50, 50)
        {
            Closable = true;
            Dragable = true;
            AddPage(0);

            // 增加背景高度以容纳新控件，从 260 增加到 300
            AddBackground(0, 0, 380, 300, 9270); // 宽度从 350 增加到 380
            AddLabel(GetCenteredX(90, 20, "张力管理面板", 380), 20, 1152, "张力管理面板");

            // 当前张力值
            AddLabel(LABEL_X, 50, 1152, $"当前张力值: {TensionManager.Tension.ToString("F3", CultureInfo.InvariantCulture)}");

            // 调整张力值 (增量/减量)
            AddLabel(LABEL_X, 80, 1152, "调整张力值:");
            AddBackground(TEXT_ENTRY_X, 75, 80, 20, 3000);
            AddTextEntry(TEXT_ENTRY_X, 77, 76, 16, 1152, TensionTextEntryID, "0.000");
            AddButton(APPLY_BUTTON_X_AFTER_TEXT_ENTRY, 75, 4023, 4025, ApplyTensionButtonID, GumpButtonType.Reply, 0);

            // 降低张力数值 (每小时)
            AddLabel(LABEL_X, 110, 1152, "降低张力数值:");
            AddBackground(TEXT_ENTRY_X, 105, 80, 20, 3000);
            AddTextEntry(TEXT_ENTRY_X, 107, 76, 16, 1152, ReductionTextEntryID, TensionManager.TensionReductionAmount.ToString("F3", CultureInfo.InvariantCulture));
            AddButton(APPLY_BUTTON_X_AFTER_TEXT_ENTRY, 105, 4023, 4025, ApplyReductionButtonID, GumpButtonType.Reply, 0);

            // *** 新增：调整生物死亡增加的张力值 ***
            AddLabel(LABEL_X, 140, 1152, "生物死亡增加张力:"); // 新标签
            AddBackground(TEXT_ENTRY_X, 135, 80, 20, 3000); // 新背景
            AddTextEntry(TEXT_ENTRY_X, 137, 76, 16, 1152, CreatureDeathIncrementTextEntryID, TensionManager.CreatureDeathTensionIncrement.ToString("F3", CultureInfo.InvariantCulture)); // 显示当前值
            AddButton(APPLY_BUTTON_X_AFTER_TEXT_ENTRY, 135, 4023, 4025, ApplyCreatureDeathIncrementButtonID, GumpButtonType.Reply, 0); // 新按钮

            // 关闭按钮
            int closeButtonY = 260; // 调整Y坐标
            int closeButtonX = 300; // 调整X坐标
            AddLabel(closeButtonX - 50, closeButtonY + 2, 1152, "关闭");
            AddButton(closeButtonX, closeButtonY, 4005, 4007, 0, GumpButtonType.Reply, 0);
        }

        private int GetCenteredX(int labelX, int labelY, string text, int backgroundWidth)
        {
            int textWidthEstimate = text.Length * 8;
            return (backgroundWidth / 2) - (textWidthEstimate / 2);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == ApplyTensionButtonID)
            {
                TextRelay entry = info.GetTextEntry(TensionTextEntryID);
                if (entry != null)
                {
                    if (double.TryParse(entry.Text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double amount))
                    {
                        TensionManager.IncreaseTension(amount);
                        from.SendMessage($"张力已调整 {amount.ToString("F3", CultureInfo.InvariantCulture)}。当前张力值：{TensionManager.Tension.ToString("F3", CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        from.SendMessage("输入的张力增量/减量无效，请输入一个有效的浮点数。");
                    }
                }
            }
            else if (info.ButtonID == ApplyReductionButtonID)
            {
                TextRelay entry = info.GetTextEntry(ReductionTextEntryID);
                if (entry != null)
                {
                    if (double.TryParse(entry.Text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double reductionAmount))
                    {
                        TensionManager.TensionReductionAmount = reductionAmount;
                        from.SendMessage($"张力降低数值已更新为每小时: {TensionManager.TensionReductionAmount.ToString("F3", CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        from.SendMessage("输入的降低数值无效，请输入一个有效的浮点数。");
                    }
                }
            }
            // *** 新增：处理生物死亡张力增量按钮的响应 ***
            else if (info.ButtonID == ApplyCreatureDeathIncrementButtonID)
            {
                TextRelay entry = info.GetTextEntry(CreatureDeathIncrementTextEntryID);
                if (entry != null)
                {
                    if (double.TryParse(entry.Text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double incrementAmount))
                    {
                        TensionManager.CreatureDeathTensionIncrement = incrementAmount;
                        from.SendMessage($"生物死亡增加的张力值已更新为: {TensionManager.CreatureDeathTensionIncrement.ToString("F3", CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        from.SendMessage("输入的生物死亡增加张力值无效，请输入一个有效的浮点数。");
                    }
                }
            }

            // 如果不是关闭按钮 (ButtonID 0)，则刷新Gump
            if (info.ButtonID != 0)
            {
                from.SendGump(new TensionAdminGump());
            }
        }
    }
}