using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

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

        // Updated sizes: double the background size and adjust all elements accordingly
        AddPage(0);
        AddBackground(0, 0, 800, 700, 9270); // Double the width and height
        
        // Change text color to light blue for NPCText
        AddHtml(40, 40, 720, 200, $"<BODY><BASEFONT COLOR=\"#ADD8E6\">{_module.NPCText}</BASEFONT></BODY>", false, true); 

        int y = 260; // Start the options lower since the text block is larger
        for (int i = 0; i < _module.Options.Count; i++)
        {
            if (_module.Options[i].Condition(_player))
            {
                AddButton(40, y, 4005, 4007, i + 1, GumpButtonType.Reply, 0); // Adjust button position
                
                // Change text color to green for the options
                AddHtml(110, y, 650, 80, $"<BODY><BASEFONT COLOR=\"#32CD32\">{_module.Options[i].Text}</BASEFONT></BODY>", false, false); 
                y += 30; // Increase vertical space between buttons for better readability
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
