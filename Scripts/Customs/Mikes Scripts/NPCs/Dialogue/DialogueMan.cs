using System;
using Server;
using Server.Mobiles;

public class DialogueMan : BaseCreature
{
    [Constructable]
    public DialogueMan() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "DialogueMan";
        Body = 400; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(100);
        SetDex(100);
        SetInt(100);

        SetHits(100);
        SetMana(100);
        SetStam(100);

        Fame = 0;
        Karma = 0;

        VirtualArmor = 0;

        Frozen = true; // Prevent NPC from moving
        CantWalk = true;
    }

    public DialogueMan(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am DialogueMan. How may I assist you today?");
        
        greeting.AddOption("Tell me about yourself.", 
            player => true, 
            player => {
                DialogueModule aboutModule = new DialogueModule("I am DialogueMan, a demonstration NPC for the new dialogue system. I can show you how different dialogue options work.");
                aboutModule.AddOption("That's interesting.", 
                    p => true, 
                    p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, aboutModule));
            });
        
        greeting.AddOption("I need healing.", 
            player => player.Hits < player.HitsMax, 
            player => {
                player.Heal(50);
                player.SendMessage("You feel refreshed!");
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });
        
        greeting.AddOption("Teach me about magic.", 
            player => player.Skills.Magery.Base >= 30.0, 
            player => {
                player.SendGump(new DialogueGump(player, MagicDialogueExtension.GetMagicModule(player)));
            });
        
        greeting.AddOption("I'm looking for work.", 
            player => true, 
            player => {
                DialogueModule workModule = new DialogueModule("I'm afraid I don't have any work for you at the moment. Perhaps you could try the job board in town?");
                workModule.AddOption("I'll do that, thanks.", 
                    p => true, 
                    p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, workModule));
            });

        greeting.AddOption("Goodbye.", 
            player => true, 
            player => player.SendMessage("DialogueMan waves goodbye."));

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}