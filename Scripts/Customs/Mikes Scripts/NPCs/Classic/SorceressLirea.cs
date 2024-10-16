using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Sorceress Lirea")]
public class SorceressLirea : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SorceressLirea() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sorceress Lirea";
        Body = 0x191; // Human female body

        // Stats
        SetStr(85);
        SetDex(70);
        SetInt(90);
        SetHits(100);

        // Appearance
        AddItem(new Robe() { Hue = 1157 });
        AddItem(new Sandals() { Hue = 1166 });
        AddItem(new Cap() { Hue = 1157 });
        AddItem(new LeatherGloves() { Hue = 1157 });

        Hue = Utility.RandomSkinHue();
        HairItemID = Utility.RandomList(0x203B, 0x203C, 0x203D); // Random female hair
        HairHue = Utility.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Sorceress Lirea. What brings you to me?");
        
        greeting.AddOption("Tell me about your abilities.",
            player => true,
            player =>
            {
                DialogueModule abilitiesModule = new DialogueModule("I am a guardian of ancient knowledge and secrets.");
                abilitiesModule.AddOption("What do you know about magic?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateMagicModule())));
                player.SendGump(new DialogueGump(player, abilitiesModule));
            });

        greeting.AddOption("Can you share your secrets?",
            player => true,
            player =>
            {
                DialogueModule secretsModule = new DialogueModule("The secrets I guard are not just of spells and potions. Seek them, and you might find greater purpose.");
                secretsModule.AddOption("Tell me more.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, secretsModule)));
                player.SendGump(new DialogueGump(player, secretsModule));
            });

        greeting.AddOption("What can you tell me about virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The virtues are the bedrock of our morality. They guide our actions. Do you seek knowledge of them?");
                virtuesModule.AddOption("Yes, I wish to learn more.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateVirtueModule())));
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("I've traveled; do you have a reward?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    player.SendMessage("For your perseverance in seeking knowledge, accept this token from a distant land.");
                    player.AddToBackpack(new MaxxiaScroll());
                    lastRewardTime = DateTime.UtcNow;
                }
            });

        greeting.AddOption("Do you have any regrets?",
            player => true,
            player =>
            {
                DialogueModule regretModule = new DialogueModule("Regrets, yes... there was someone I once loved dearly.");
                regretModule.AddOption("What happened?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateBoyfriendModule())));
                player.SendGump(new DialogueGump(player, regretModule));
            });

        return greeting;
    }

    private DialogueModule CreateMagicModule()
    {
        DialogueModule magicModule = new DialogueModule("Magic is the essence of life, connecting us to the world.");
        magicModule.AddOption("What about the essence of magic?",
            pl => true,
            pl =>
            {
                DialogueModule essenceModule = new DialogueModule("The essence of magic allows us to manipulate the elements and heal wounds.");
                pl.SendGump(new DialogueGump(pl, essenceModule));
            });
        return magicModule;
    }

    private DialogueModule CreateVirtueModule()
    {
        DialogueModule virtueModule = new DialogueModule("There are many virtues like Compassion, Honesty, and Valor.");
        virtueModule.AddOption("What are the paths to enlightenment?",
            pl => true,
            pl =>
            {
                DialogueModule pathsModule = new DialogueModule("Each virtue is a path. Which one resonates with your heart?");
                pl.SendGump(new DialogueGump(pl, pathsModule));
            });
        return virtueModule;
    }

    private DialogueModule CreateBoyfriendModule()
    {
        DialogueModule boyfriendModule = new DialogueModule("His name was Eldrin, a charming rogue with a heart of gold. But he was reckless.");
        
        boyfriendModule.AddOption("Why did you turn him into a toad?",
            pl => true,
            pl =>
            {
                DialogueModule reasonModule = new DialogueModule("He broke my trust one too many times. One fateful night, in a fit of anger, I transformed him into a toad. A lesson he needed to learn.");
                reasonModule.AddOption("What happened next?",
                    pla => true,
                    pla => pla.SendGump(new DialogueGump(pla, CreateCageModule())));
                pl.SendGump(new DialogueGump(pl, reasonModule));
            });

        return boyfriendModule;
    }

    private DialogueModule CreateCageModule()
    {
        DialogueModule cageModule = new DialogueModule("I kept him in a small cage for a while, to teach him humility. It was difficult to see him that way, but he needed to reflect.");
        
        cageModule.AddOption("Did you ever talk to him?",
            pl => true,
            pl =>
            {
                DialogueModule talkModule = new DialogueModule("Yes, I would speak to him softly, reminding him of his past mistakes. Sometimes he would croak back, as if he understood.");
                talkModule.AddOption("What did you say?",
                    pla => true,
                    pla =>
                    {
                        DialogueModule conversationModule = new DialogueModule("I told him that true strength lies in responsibility and honesty, not in charming words.");
                        conversationModule.AddOption("Did he change?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule changeModule = new DialogueModule("I hoped he would, but after some time, I realized I had to set him free.");
                                changeModule.AddOption("What did you do next?",
                                    plaaa => true,
                                    plaaa => pl.SendGump(new DialogueGump(plaaa, CreateReleaseModule())));
                                pl.SendGump(new DialogueGump(pl, changeModule));
                            });
                        pla.SendGump(new DialogueGump(pla, conversationModule));
                    });
                pl.SendGump(new DialogueGump(pl, talkModule));
            });

        return cageModule;
    }

    private DialogueModule CreateReleaseModule()
    {
        DialogueModule releaseModule = new DialogueModule("I released him in the swamps, where he could start anew. It was bittersweet, but I knew it was for the best.");
        
        releaseModule.AddOption("Do you regret letting him go?",
            pl => true,
            pl =>
            {
                DialogueModule regretReleaseModule = new DialogueModule("Sometimes, yes. I wonder if he learned from his mistakes or if he simply hopped away to another adventure.");
                regretReleaseModule.AddOption("Would you take him back?",
                    pla => true,
                    pla =>
                    {
                        DialogueModule returnModule = new DialogueModule("If he returned as a changed man, perhaps. But if he remained the same, I would not.");
                        pla.SendGump(new DialogueGump(pla, returnModule));
                    });
                pl.SendGump(new DialogueGump(pl, regretReleaseModule));
            });

        return releaseModule;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }

    public SorceressLirea(Serial serial) : base(serial) { }
}
