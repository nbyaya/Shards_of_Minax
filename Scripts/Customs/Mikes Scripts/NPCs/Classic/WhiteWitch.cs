using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of The White Witch")]
public class WhiteWitch : BaseCreature
{
    private DateTime lastSpecialBrewTime;
    private DateTime lastMysticKeyTime;
    private DateTime lastScrollTime;
    private DateTime lastMoonflowerTime;
    private DateTime lastCrystalsTime;

    [Constructable]
    public WhiteWitch() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "The White Witch";
        Body = 0x191; // Human female body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(100);
        SetHits(75);

        // Appearance
        AddItem(new Robe() { Hue = 1150 });
        AddItem(new Boots() { Hue = 1150 });
        AddItem(new WizardsHat() { Hue = 1150 });
        AddItem(new Spellbook() { Name = "Jadis's Tome" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastSpecialBrewTime = DateTime.MinValue;
        lastMysticKeyTime = DateTime.MinValue;
        lastScrollTime = DateTime.MinValue;
        lastMoonflowerTime = DateTime.MinValue;
        lastCrystalsTime = DateTime.MinValue;
    }

	public WhiteWitch(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am The White Witch, once Queen of Narnia. How may I assist you today?");

        greeting.AddOption("Tell me about your time as Queen of Narnia.",
            player => true,
            player =>
            {
                DialogueModule queenModule = new DialogueModule("Ah, my reign was both glorious and tumultuous. I ruled a land of magic and wonder, yet shadows loomed at every turn. What would you like to know?");
                
                queenModule.AddOption("What was Narnia like?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule narniaModule = new DialogueModule("Narnia was a realm of breathtaking beauty—rolling hills, enchanted forests, and mythical creatures roamed free. Each season brought its own magic, but I always preferred winter, as it echoed my own nature.");
                        narniaModule.AddOption("Did you have allies?",
                            p => true,
                            p =>
                            {
                                DialogueModule alliesModule = new DialogueModule("Indeed! I had many allies, including the loyal wolves and the cunning fauns. But I also had foes, such as Aslan, the great lion, who sought to undermine my rule.");
                                alliesModule.AddOption("Tell me more about Aslan.",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule aslanModule = new DialogueModule("Aslan was a powerful and noble creature. He symbolized hope and renewal, opposing my eternal winter. Our encounters were always filled with tension and magic.");
                                        aslanModule.AddOption("Did you ever defeat him?",
                                            pw => true,
                                            pw =>
                                            {
                                                DialogueModule defeatModule = new DialogueModule("Defeat was never truly in my grasp, for his power is tied to the very essence of Narnia. Each victory I thought I achieved was met with his inevitable return.");
                                                p.SendGump(new DialogueGump(p, defeatModule));
                                            });
                                        p.SendGump(new DialogueGump(p, aslanModule));
                                    });
                                p.SendGump(new DialogueGump(p, alliesModule));
                            });
                        narniaModule.AddOption("What about your enemies?",
                            p => true,
                            p =>
                            {
                                DialogueModule enemiesModule = new DialogueModule("My greatest enemies were the Pevensie siblings. They dared to challenge my authority, wielding the hope of the people as their weapon. I respected their courage, but I was determined to crush their rebellion.");
                                enemiesModule.AddOption("How did you deal with them?",
                                    ple => true,
                                    ple =>
                                    {
                                        DialogueModule dealWithThemModule = new DialogueModule("I sent my fiercest beasts after them, hoping to instill fear. Yet, they had a way of rallying the creatures of Narnia to their side, stirring the hearts of even my loyal subjects.");
                                        pl.SendGump(new DialogueGump(pl, dealWithThemModule));
                                    });
                                p.SendGump(new DialogueGump(p, enemiesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, narniaModule));
                    });

                queenModule.AddOption("What was your greatest triumph?",
                    playert => true,
                    playert =>
                    {
                        DialogueModule triumphModule = new DialogueModule("My greatest triumph was the expansion of my realm, stretching the borders of winter across all of Narnia. The lands flourished under my cold embrace, and the magic of winter became my signature.");
                        triumphModule.AddOption("And your greatest failure?",
                            ply => true,
                            ply =>
                            {
                                DialogueModule failureModule = new DialogueModule("My greatest failure was underestimating the power of unity among the creatures of Narnia. When they banded together, my ice began to melt, and their hopes rekindled.");
                                ply.SendGump(new DialogueGump(ply, failureModule));
                            });
                        player.SendGump(new DialogueGump(player, triumphModule));
                    });

                queenModule.AddOption("What do you miss the most?",
                    playerx => true,
                    playerx =>
                    {
                        DialogueModule missModule = new DialogueModule("I miss the thrill of the chase and the power that surged through me as Queen. The whispers of magic in the air and the sheer force of my will shaping Narnia's fate.");
                        player.SendGump(new DialogueGump(player, missModule));
                    });

                player.SendGump(new DialogueGump(player, queenModule));
            });

        greeting.AddOption("What about your potions?",
            player => true,
            player =>
            {
                DialogueModule potionsModule = new DialogueModule("Ah, potions! My specialty. I can craft many, from healing to the arcane. Some come at a price, while others require a quest.");
                potionsModule.AddOption("What quests do you have?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateQuestModule())); });
                player.SendGump(new DialogueGump(player, potionsModule));
            });

        greeting.AddOption("What about the mystic key?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastMysticKeyTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no mystic key for you at the moment. Please return later.");
                }
                else
                {
                    DialogueModule mysticKeyModule = new DialogueModule("Not just a physical object, but a metaphorical one. Assist me in a task, and I might bestow upon you a 'mystic key'.");
                    lastMysticKeyTime = DateTime.UtcNow; // Update time
                    player.SendGump(new DialogueGump(player, mysticKeyModule));
                }
            });

        greeting.AddOption("Tell me about the Lost Library.",
            player => true,
            player =>
            {
                DialogueModule libraryModule = new DialogueModule("Ah, the Lost Library. A place of great knowledge. Retrieve a 'forgotten scroll' for me, and I could guide you there.");
                player.SendGump(new DialogueGump(player, libraryModule));
            });

        // Additional options can be added similarly...

        return greeting;
    }

    private DialogueModule CreateQuestModule()
    {
        DialogueModule questModule = new DialogueModule("I have several tasks that could use your help.");

        questModule.AddOption("What about the special brew?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastSpecialBrewTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no special brew ready at the moment. Please return later.");
                }
                else
                {
                    DialogueModule specialBrewModule = new DialogueModule("My special brew requires a rare 'moonflower'. Bring it to me, and I will reward you with this brew.");
                    lastSpecialBrewTime = DateTime.UtcNow; // Update time
                    player.SendGump(new DialogueGump(player, specialBrewModule));
                }
            });

        questModule.AddOption("What about the moonflower?",
            player => true,
            player =>
            {
                DialogueModule moonflowerModule = new DialogueModule("Bring me the moonflower from the Moonlit Grove, and I will reward you with the special brew.");
                player.SendGump(new DialogueGump(player, moonflowerModule));
            });

        questModule.AddOption("What about the enchanted crystals?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastCrystalsTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no use for more crystals right now. Please return later.");
                }
                else
                {
                    DialogueModule crystalsModule = new DialogueModule("These crystals resonate with ancient magic. They are found deep within the Mystic Caves. Bring me a handful, and I shall reward you with the mystic key and another gift.");
                    lastCrystalsTime = DateTime.UtcNow; // Update time
                    player.SendGump(new DialogueGump(player, crystalsModule));
                }
            });

        questModule.AddOption("I have to go.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return questModule;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(lastSpecialBrewTime);
        writer.Write(lastMysticKeyTime);
        writer.Write(lastScrollTime);
        writer.Write(lastMoonflowerTime);
        writer.Write(lastCrystalsTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastSpecialBrewTime = reader.ReadDateTime();
        lastMysticKeyTime = reader.ReadDateTime();
        lastScrollTime = reader.ReadDateTime();
        lastMoonflowerTime = reader.ReadDateTime();
        lastCrystalsTime = reader.ReadDateTime();
    }
}
