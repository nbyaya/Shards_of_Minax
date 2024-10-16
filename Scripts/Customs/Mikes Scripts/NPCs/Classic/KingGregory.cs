using System;
using Server;
using Server.Mobiles;
using Server.Items;

[CorpseName("the corpse of King Gregory")]
public class KingGregory : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public KingGregory() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "King Gregory";
        Body = 0x190; // Human male body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

        // Initialize stats
        SetStr(110);
        SetDex(110);
        SetInt(80);
        SetHits(85);

        // Appearance
        AddItem(new StuddedLegs() { Hue = 1428 });
        AddItem(new StuddedChest() { Hue = 1428 });
        AddItem(new StuddedGloves() { Hue = 1428 });
        AddItem(new Bandana() { Hue = 1428 });
        AddItem(new Boots() { Hue = 1428 });
        AddItem(new Bow() { Name = "King Gregory's Bow" });

        lastRewardTime = DateTime.MinValue; // Initialize the last reward time
    }

    public KingGregory(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am King Gregory, ruler of this land. How may I assist you today?");

        greeting.AddOption("Tell me about the virtues.",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The virtues are the foundation of a just society. Do you understand their importance?");
                virtuesModule.AddOption("Yes, I understand.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule virtuesDetail = new DialogueModule("The virtues are Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each one is vital for a harmonious world. Would you like to learn about a specific virtue?");
                        virtuesDetail.AddOption("Tell me about Honesty.",
                            pl2 => true,
                            pl2 =>
                            {
                                pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Honesty is the bedrock of trust. Without it, society crumbles. Always strive to speak the truth, even when it is difficult.")));
                            });
                        virtuesDetail.AddOption("Tell me about Compassion.",
                            pl2 => true,
                            pl2 =>
                            {
                                pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Compassion is the ability to empathize with others. It compels us to act for the welfare of those in need.")));
                            });
                        virtuesDetail.AddOption("Tell me about Valor.",
                            pl2 => true,
                            pl2 =>
                            {
                                pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Valor is not the absence of fear but the determination to face it. True bravery is standing up for what is right.")));
                            });
                        // Add more virtues similarly...
                        player.SendGump(new DialogueGump(player, virtuesDetail));
                    });
                virtuesModule.AddOption("No, I need more information.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("You must strive to embody the virtues in your daily life; they guide our actions.")));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("What about your health?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My health is as strong as my commitment to the virtues. I keep my mind and body fit for the burdens of leadership.")));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My duty is to uphold the virtues and ensure justice prevails in my kingdom. Each day brings new challenges.")));
            });

        greeting.AddOption("Tell me about Britannia.",
            player => true,
            player =>
            {
                DialogueModule britanniaModule = new DialogueModule("Britannia is a land of magic and mystery, filled with heroes and legends. It consists of eight cities, each representing one of the virtues. Do you wish to learn about a specific city?");
                britanniaModule.AddOption("Tell me about Britain.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Britain is the capital of the realm, bustling with trade and culture. It is home to the royal family and many adventurers.")));
                    });
                britanniaModule.AddOption("Tell me about Yew.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Yew is known for its ancient forests and druidic traditions. It is a place of peace and spirituality.")));
                    });
                britanniaModule.AddOption("Tell me about Trinsic.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Trinsic is a city of honor and valor, known for its knights and their unyielding spirit. It stands strong against darkness.")));
                    });
                // Add more cities similarly...
                player.SendGump(new DialogueGump(player, britanniaModule));
            });

        greeting.AddOption("What about the Ankh of Virtues?",
            player => true,
            player =>
            {
                DialogueModule artifactModule = new DialogueModule("The Ankh of Virtues amplifies the virtues or reveals deep vices. I fear it may be misused. If you retrieve it, I shall reward you handsomely.");
                artifactModule.AddOption("What is the threat?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Dark forces emerge from the Abyss, seeking to corrupt the land. They are drawn to the Ankh's power and may be behind its theft.")));
                    });
                artifactModule.AddOption("How can I help retrieve the Ankh?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule helpModule = new DialogueModule("You must venture into the depths of the Abyss, where the forces of darkness dwell. Be wary; it is a perilous journey.");
                        helpModule.AddOption("I will gather a party to assist me.",
                            pl2 => true,
                            pl2 =>
                            {
                                pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Wise choice! Strength in numbers will aid you greatly.")));
                            });
                        helpModule.AddOption("I will go alone; I am prepared.",
                            pl2 => true,
                            pl2 =>
                            {
                                pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Bravery is commendable, but be cautious. The Abyss is no place for the unprepared.")));
                            });
                        player.SendGump(new DialogueGump(player, helpModule));
                    });
                player.SendGump(new DialogueGump(player, artifactModule));
            });

        greeting.AddOption("Can you reward me for embodying the virtues?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
                else
                {
                    player.AddToBackpack(new LowerAttackAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.SendGump(new DialogueGump(player, new DialogueModule("Take this as a reward for your commitment to the virtues. May it serve you well!")));
                }
            });

        greeting.AddOption("What else can you teach me?",
            player => true,
            player =>
            {
                DialogueModule teachingModule = new DialogueModule("Knowledge is power. I can teach you about the history of our land, the ways of the virtues, or even the threats we face.");
                teachingModule.AddOption("Tell me about the history of Britannia.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Britannia has a rich history filled with heroes and great wars. Legends speak of the First Age, where mighty champions fought against darkness.")));
                    });
                teachingModule.AddOption("What threats do we face?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Many threats loom over our land—dark sorcery, monsters from the Abyss, and even betrayals from within. Vigilance is essential.")));
                    });
                player.SendGump(new DialogueGump(player, teachingModule));
            });

        return greeting;
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
}
