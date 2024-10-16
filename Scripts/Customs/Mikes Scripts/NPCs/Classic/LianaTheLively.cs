using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Liana the Lively")]
public class LianaTheLively : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LianaTheLively() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Liana the Lively";
        Body = 0x191; // Human female body

        // Stats
        SetStr(82);
        SetDex(42);
        SetInt(120);
        SetHits(72);

        // Appearance
        AddItem(new Robe() { Hue = 64 });
        AddItem(new Boots() { Hue = 2126 });
        AddItem(new LeatherGloves() { Name = "Liana's Mixing Mitts" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public LianaTheLively(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Liana the Lively, the alchemist. What do you want?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("Health? What's it to you? I'm not your nurse."))); });

        greeting.AddOption("What do you do?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("My job? I'm an alchemist, not that it's any of your business."))); });

        greeting.AddOption("Do you have wisdom to share?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("Knowledge is power, and I have plenty of it. Do you seek wisdom?"))); });

        greeting.AddOption("Yes, I seek wisdom.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("Wisdom, huh? You don't strike me as the wise type."))); });

        greeting.AddOption("What about your potions?",
            player => true,
            player => 
        {
            DialogueModule potionsModule = new DialogueModule("Ah, the legendary Elixir of Life. A potion that can heal any ailment, but the ingredients are hard to come by. If you fetch them for me, there might be a reward in it for you.");
            potionsModule.AddOption("What ingredients do you need?",
                pl => true,
                pl => 
                {
                    DialogueModule ingredientsModule = new DialogueModule("I need the petal of a Moonflower, essence of a phoenix, and a vial of water from the Whispering Spring. Not easy to come by, but the reward will be worth it.");
                    ingredientsModule.AddOption("What reward will I receive?",
                        pla => true,
                        pla => 
                        {
                            TimeSpan cooldown = TimeSpan.FromMinutes(10);
                            if (DateTime.UtcNow - lastRewardTime < cooldown)
                            {
                                pla.SendGump(new DialogueGump(pla, new DialogueModule("I have no reward right now. Please return later.")));
                            }
                            else
                            {
                                pla.AddToBackpack(new HealingAugmentCrystal()); // Give the reward
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                pla.SendGump(new DialogueGump(pla, new DialogueModule("From rare herbs to mystical artifacts, the land has given me many treasures. Here, have a sample.")));
                            }
                        });
                    ingredientsModule.AddOption("Where can I find a Moonflower?",
                        pla => true,
                        pla => { pla.SendGump(new DialogueGump(pla, new DialogueModule("Moonflowers bloom under the light of the full moon, deep within the Whispering Forest. Watch out for the creatures that guard them!"))); });

                    ingredientsModule.AddOption("What is the essence of a phoenix?",
                        pla => true,
                        pla => { pla.SendGump(new DialogueGump(pla, new DialogueModule("Essence of a phoenix is a rare ingredient. You must find a phoenix and capture its tears, which are said to hold its essence. Tread carefully; they are fierce!"))); });

                    ingredientsModule.AddOption("What about the Whispering Spring?",
                        pla => true,
                        pla => { pla.SendGump(new DialogueGump(pla, new DialogueModule("The Whispering Spring is a mystical place, hidden away in the hills. Listen for the whispers of the water to guide you, but beware of the trolls that lurk nearby."))); });

                    pl.SendGump(new DialogueGump(pl, ingredientsModule));
                });
            player.SendGump(new DialogueGump(player, potionsModule));
        });

        greeting.AddOption("What do you know about nature?",
            player => true,
            player => 
        {
            DialogueModule natureModule = new DialogueModule("Nature is a powerful force. She whispers her secrets to those who listen. In return, I respect and protect her.");
            natureModule.AddOption("How do you protect nature?",
                pl => true,
                pl => 
                {
                    DialogueModule protectionModule = new DialogueModule("Over the years, I've saved many creatures and plants from harm. In gratitude, the land often bestows gifts upon me.");
                    protectionModule.AddOption("What kind of gifts?",
                        pla => true,
                        pla => { pla.SendGump(new DialogueGump(pla, new DialogueModule("From rare herbs to mystical artifacts, I often receive treasures. If you help me, I might share some with you."))); });
                    protectionModule.AddOption("Can I help you?",
                        pla => true,
                        pla => 
                        {
                            DialogueModule helpModule = new DialogueModule("Indeed! If you can bring me those rare ingredients, I'd be grateful. There's much knowledge to share in return.");
                            helpModule.AddOption("I'll gather the ingredients!",
                                plq => true,
                                plq => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Wonderful! I look forward to your return."))); });
                            helpModule.AddOption("What if I can't find them?",
                                plw => true,
                                plw => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Don't worry. Sometimes, patience is the key. Explore, and you might stumble upon them."))); });
                            pla.SendGump(new DialogueGump(pla, helpModule));
                        });
                    pl.SendGump(new DialogueGump(pl, protectionModule));
                });
            player.SendGump(new DialogueGump(player, natureModule));
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
