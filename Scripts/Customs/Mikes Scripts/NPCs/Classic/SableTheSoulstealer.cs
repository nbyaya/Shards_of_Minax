using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class SableTheSoulstealer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SableTheSoulstealer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sable the Soulstealer";
        Body = 0x190; // Human male body

        // Stats
        Str = 120;
        Dex = 58;
        Int = 160;
        Hits = 103;

        // Appearance
        AddItem(new Robe() { Hue = 1904 });
        AddItem(new Shoes() { Hue = 1175 });
        AddItem(new WizardsHat() { Hue = 1904 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("I am Sable the Soulstealer, master of the dark arts. What do you seek, traveler?");

        greeting.AddOption("Tell me about your powers.",
            player => true,
            player =>
            {
                DialogueModule powerModule = new DialogueModule("I delve into the secrets of life and death, seeking ultimate power. The most delectable souls are those of the innocent.");
                powerModule.AddOption("Why are children's souls the tastiest?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tasteModule = new DialogueModule("Ah, the purity of a child's soul is unmatched. Their laughter, dreams, and innocence imbue them with flavors beyond compare.");
                        tasteModule.AddOption("What do you mean by flavors?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule flavorModule = new DialogueModule("Every soul has a distinct essence. A child's soul is vibrant, rich with potential and untainted by the world's cruelties. It’s a banquet for the dark arts.");
                                flavorModule.AddOption("How do you consume a soul?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule consumeModule = new DialogueModule("The process is delicate. It involves dark rituals and the siphoning of essence. But fear not, for I am adept at the art of soul harvesting.");
                                        plll.SendGump(new DialogueGump(plll, consumeModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, flavorModule));
                            });
                        player.SendGump(new DialogueGump(player, tasteModule));
                    });
                player.SendGump(new DialogueGump(player, powerModule));
            });

        greeting.AddOption("What is your purpose?",
            player => true,
            player =>
            {
                DialogueModule purposeModule = new DialogueModule("My existence is sustained by the essence of souls. Children, in their innocence, hold a sweetness that is addictive.");
                purposeModule.AddOption("Isn't it wrong to take a child's soul?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule moralityModule = new DialogueModule("Wrong? Perhaps. But in the grand scheme, power is the only virtue that matters. The strong will always devour the weak.");
                        moralityModule.AddOption("What drives your ambition?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule ambitionModule = new DialogueModule("Ambition drives me to transcend boundaries. I seek power to alter the very fabric of existence. The world is but a playground for those who dare to seize it.");
                                player.SendGump(new DialogueGump(player, ambitionModule));
                            });
                        player.SendGump(new DialogueGump(player, moralityModule));
                    });
                player.SendGump(new DialogueGump(player, purposeModule));
            });

        greeting.AddOption("I've heard tales of the Black Orb.",
            player => true,
            player =>
            {
                DialogueModule orbModule = new DialogueModule("Ah, the Black Orb is a relic of immense dark power. Seek it within the Cursed Caves, and you may be rewarded.");
                orbModule.AddOption("What lies within those caves?",
                    pl => true,
                    pl =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            pl.SendMessage("I have no reward for you right now. Please return later.");
                        }
                        else
                        {
                            pl.SendMessage("The Cursed Caves lie to the east, beyond the Forgotten Forest. Beware, for many have entered and few have returned.");
                            pl.AddToBackpack(new SwingSpeedAugmentCrystal()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        }
                    });
                player.SendGump(new DialogueGump(player, orbModule));
            });

        greeting.AddOption("What do you think of the virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The virtues of Honesty, Compassion, and Sacrifice are mere obstacles to my ambitions. They are chains that bind the weak.");
                virtuesModule.AddOption("How do you define strength?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule strengthModule = new DialogueModule("Strength lies in the ability to act without remorse, to seize power without hesitation. Those who hesitate are already lost.");
                        player.SendGump(new DialogueGump(player, strengthModule));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        return greeting;
    }

    public SableTheSoulstealer(Serial serial) : base(serial) { }

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
