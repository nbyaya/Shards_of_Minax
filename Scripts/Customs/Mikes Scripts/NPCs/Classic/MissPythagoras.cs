using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Miss Pythagoras")]
public class MissPythagoras : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MissPythagoras() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Miss Pythagoras";
        Body = 0x191; // Human female body

        // Stats
        SetStr(85);
        SetDex(70);
        SetInt(105);
        SetHits(65);

        // Appearance
        AddItem(new Kilt() { Hue = 1154 });
        AddItem(new Tunic() { Hue = 1154 });
        AddItem(new Shoes() { Hue = 1154 });
        AddItem(new Spellbook() { Name = "Pythagoras Theorems" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Miss Pythagoras, a philosopher of the land. What wisdom do you seek?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("My health is but a fleeting measure of my existence, much like a candle's flicker in the wind."))));

        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I delve into contemplation and the pursuit of knowledge, exploring the very fabric of existence."))));

        greeting.AddOption("What can you tell me about virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The eight virtues are the pillars of a virtuous life: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
                virtuesModule.AddOption("Tell me about Honesty.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Honesty is the foundation of trust. Without it, relationships crumble like ancient ruins."))));
                virtuesModule.AddOption("What about Compassion?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Compassion is the ability to empathize with others, to feel their joys and sorrows as if they were your own."))));
                virtuesModule.AddOption("Tell me about Valor.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Valor is the courage to face one's fears and stand firm in the face of adversity."))));
                virtuesModule.AddOption("What is Justice?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Justice is the pursuit of fairness, ensuring that everyone receives their due."))));
                virtuesModule.AddOption("What about Sacrifice?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Sacrifice often requires giving up something dear for the greater good."))));
                virtuesModule.AddOption("Tell me about Honor.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Honor is living by one's principles, even when it is difficult to do so."))));
                virtuesModule.AddOption("What is Spirituality?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Spirituality connects us to something greater than ourselves, fostering a sense of belonging."))));
                virtuesModule.AddOption("And Humility?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Humility teaches us to recognize our limitations and value the contributions of others."))));
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("What can you tell me about Pythagoras?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I am named after the great Pythagoras, renowned for his contributions to mathematics and philosophy. Have you studied geometry, traveler?"))));

        greeting.AddOption("Tell me about geometry.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Geometry, the study of shapes and sizes, is essential for understanding the world around us. It is the language of architecture and art."))));

        greeting.AddOption("What about the nature of time?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Time is a relentless river, ever flowing. We can neither hold nor change its course, only learn to navigate its waters."))));

        greeting.AddOption("Do you have a reward for me?",
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
                    player.SendGump(new DialogueGump(player, new DialogueModule("Honor is not merely a virtue but a way of life. If you can show me an act of honor, I might have a reward for you.")));
                    // Give the reward, e.g., from a backpack item check
                    player.AddToBackpack(new MaxxiaScroll());
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            });

        greeting.AddOption("What do you think of the stars?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("The stars are ancient guides, each shining with stories untold. They remind us of our dreams and the vastness of the universe."))));

        greeting.AddOption("How do you define wisdom?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Wisdom is the ability to apply knowledge in a way that is beneficial, to discern the right path among many."))));

        greeting.AddOption("What is your philosophy on life?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Life is a journey of learning and growth. Each experience, whether joyous or sorrowful, contributes to our understanding."))));

        greeting.AddOption("What advice would you give to a traveler?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Always remain curious and open to new experiences. The world is a tapestry of wonders waiting to be explored."))));

        return greeting;
    }

    public MissPythagoras(Serial serial) : base(serial) { }

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
