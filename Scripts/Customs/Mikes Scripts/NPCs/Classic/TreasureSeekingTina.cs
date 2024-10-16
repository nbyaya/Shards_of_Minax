using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class TreasureSeekingTina : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public TreasureSeekingTina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Treasure-seeking Tina";
        Body = 0x191; // Human female body

        // Stats
        SetStr(125);
        SetDex(60);
        SetInt(30);
        SetHits(88);

        // Appearance
        AddItem(new ShortPants() { Hue = 1157 });
        AddItem(new FancyShirt() { Hue = 1156 });
        AddItem(new Boots() { Hue = 1171 });
        AddItem(new TricorneHat() { Hue = 1155 });
        AddItem(new ThinLongsword() { Name = "Tina's Treasure Blade" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public TreasureSeekingTina(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Arr, I be Treasure-seeking Tina, scourin' the seas for hidden riches! I've been searchin' for the lost treasure of the Emperor of the Planar Imperium. What brings ye here?");
        
        greeting.AddOption("Tell me about the treasure of the Emperor.",
            player => true,
            player =>
            {
                DialogueModule treasureModule = new DialogueModule("Ah, the lost treasure! It is said to be beyond compare, filled with gold, gems, and artifacts of unimaginable power. Many have sought it, but none have returned!");
                treasureModule.AddOption("Why can't you find it?",
                    p => true,
                    p =>
                    {
                        DialogueModule reasonModule = new DialogueModule("The clues are scattered across the realms, hidden away by powerful enchantments. I've followed many leads, but they always end in dead ends or dangerous traps.");
                        reasonModule.AddOption("What clues have you found?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule cluesModule = new DialogueModule("One clue spoke of a celestial map hidden within the Whispering Caverns, guarded by ancient sentinels. Another mentioned a tome of secrets in the Library of Eldoria.");
                                cluesModule.AddOption("I can help you find these clues.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Ye are brave to offer! Together, we might uncover the path to the treasure!");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                cluesModule.AddOption("That sounds dangerous.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, cluesModule));
                            });
                        reasonModule.AddOption("What traps have you encountered?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule trapsModule = new DialogueModule("Many traps await the unwary: illusions that lead ye astray, guardians of stone that awaken with the slightest sound, and puzzles that challenge the mind.");
                                trapsModule.AddOption("I can handle traps.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Then ye have the spirit of an adventurer! We shall forge ahead and confront whatever lies in our path.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                trapsModule.AddOption("Maybe I'll think twice.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, trapsModule));
                            });
                        player.SendGump(new DialogueGump(player, reasonModule));
                    });

                treasureModule.AddOption("What is the significance of this treasure?",
                    playera => true,
                    playera =>
                    {
                        DialogueModule significanceModule = new DialogueModule("The Emperor's treasure holds not just wealth but also artifacts of great power. Some say it could grant wishes or even alter the fabric of reality!");
                        significanceModule.AddOption("What kind of artifacts?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule artifactModule = new DialogueModule("Artifacts like the Scepter of Realms, said to control the tides of fate, or the Amulet of Eternity, granting its bearer a glimpse into the future.");
                                artifactModule.AddOption("That sounds incredible!",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                player.SendGump(new DialogueGump(player, artifactModule));
                            });
                        player.SendGump(new DialogueGump(player, significanceModule));
                    });

                player.SendGump(new DialogueGump(player, treasureModule));
            });

        greeting.AddOption("Do you have any treasures to share?",
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
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.AddToBackpack(new RingSlotChangeDeed());
                    player.SendMessage("If ye help me find the missing piece of me map and locate the Heart, I promise ye a share of the treasure and a surprise gift from me own collection. A sample for ye.");
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("Tell me about Captain Blackbeard.",
            player => true,
            player =>
            {
                DialogueModule blackbeardModule = new DialogueModule("Captain Blackbeard be a dreaded pirate, and his ghost ship still haunts these waters. Some say he knows the way to the Heart of the Ocean, but he's not one to share secrets lightly.");
                blackbeardModule.AddOption("How can I find him?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule findModule = new DialogueModule("They say he appears during fierce storms or in the depths of the Abyssal Sea. Ye must be prepared to face peril if ye wish to seek him out!");
                        findModule.AddOption("I'm not afraid of danger.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Then gather your courage, and we may yet uncover the secrets he guards!");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, findModule));
                    });
                player.SendGump(new DialogueGump(player, blackbeardModule));
            });

        greeting.AddOption("What do you seek?",
            player => true,
            player =>
            {
                DialogueModule seekModule = new DialogueModule("Me job be huntin' for buried treasures, and defendin' me ship from scallywags! But the treasure of the Emperor eludes me, and it be weighin' heavy on me heart.");
                seekModule.AddOption("I can help you find it.",
                    pla => true,
                    pla =>
                    {
                        pla.SendMessage("If ye truly wish to aid me, we could scour the realms together! Time be of the essence!");
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, seekModule));
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}
