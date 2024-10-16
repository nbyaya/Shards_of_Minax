using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network; // Add this line to include the NetState class

public class DialogueModule
{
    public string NPCText { get; set; }
    public List<DialogueOption> Options { get; set; }

    public DialogueModule(string npcText)
    {
        NPCText = npcText;
        Options = new List<DialogueOption>();
    }

    public void AddOption(string text, Func<PlayerMobile, bool> condition, Action<PlayerMobile> action)
    {
        Options.Add(new DialogueOption(text, condition, action));
    }
}

public class DialogueOption
{
    public string Text { get; set; }
    public Func<PlayerMobile, bool> Condition { get; set; }
    public Action<PlayerMobile> Action { get; set; }

    public DialogueOption(string text, Func<PlayerMobile, bool> condition, Action<PlayerMobile> action)
    {
        Text = text;
        Condition = condition;
        Action = action;
    }
}

public class DialogueGump : Gump
{
    private DialogueModule _module;
    private PlayerMobile _player;

    public DialogueGump(PlayerMobile player, DialogueModule module) : base(100, 100)
    {
        _player = player;
        _module = module;

        Closable = true;
        Disposable = true;
        Dragable = true;
        Resizable = false;

        AddPage(0);
        AddBackground(0, 0, 400, 350, 9200);
        AddHtml(20, 20, 360, 100, _module.NPCText, false, true);

        int y = 130;
        for (int i = 0; i < _module.Options.Count; i++)
        {
            if (_module.Options[i].Condition(_player))
            {
                AddButton(20, y, 4005, 4007, i + 1, GumpButtonType.Reply, 0);
                AddHtml(55, y, 325, 40, _module.Options[i].Text, false, false);
                y += 40;
            }
        }
    }

	public override void OnResponse(NetState sender, RelayInfo info)
    {
        PlayerMobile player = sender.Mobile as PlayerMobile;

        if (player == null)
            return;

        int button = info.ButtonID - 1;

        if (button >= 0 && button < _module.Options.Count)
        {
            _module.Options[button].Action(player);
        }
    }
}

