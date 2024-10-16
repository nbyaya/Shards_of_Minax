using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Mystic Miro")]
public class MysticMiro : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MysticMiro() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mystic Miro";
        Body = 0x190; // Human male body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

         AddItem(new Robe() { Hue = 1908 }); // Dark robe
         AddItem(new Sandals() { Hue = 1908 });
         AddItem(new Spellbook() { Name = "Miro's Mystic Tome" });

        lastRewardTime = DateTime.MinValue;
    }

    public MysticMiro(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Mystic Miro, seeker of knowledge. What wisdom do you seek?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutMe = new DialogueModule("I am a mystic, delving into the secrets of the universe. Many know me as a guardian of ancient knowledge, while others seek my counsel on their journeys. Have you heard of the ancient scrolls that hold forgotten lore?");
                aboutMe.AddOption("What scrolls do you speak of?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule scrollsModule = new DialogueModule("The ancient scrolls are filled with prophecies and knowledge of the past. They speak of the chosen ones who will restore balance to the realms.");
                        pl.SendGump(new DialogueGump(pl, scrollsModule));
                    });
                player.SendGump(new DialogueGump(player, aboutMe));
            });

        greeting.AddOption("What are the virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The virtues are the foundation of a noble life. Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility guide us toward enlightenment. Reflect on them as you journey forth.");
                virtuesModule.AddOption("Can you elaborate on each virtue?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule detailedVirtues = new DialogueModule("Honesty ensures trust, Compassion binds us to one another, Valor gives us courage, Justice keeps us balanced, Sacrifice shows our love, Honor defines our character, Spirituality connects us to the divine, and Humility grounds us.");
                        pl.SendGump(new DialogueGump(pl, detailedVirtues));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Can you teach me about the spirit realm?",
            player => true,
            player =>
            {
                DialogueModule spiritModule = new DialogueModule("The spirit realm is an ethereal plane filled with both beauty and danger. Only those with pure hearts can navigate its depths. I can teach you a ritual to glimpse into it, should you wish.");
                spiritModule.AddOption("Yes, please teach me.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Mystic Miro begins to teach you the ways of the spirit realm.");
                        // Add additional lore or mechanics related to the ritual here
                    });
                player.SendGump(new DialogueGump(player, spiritModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
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
                    player.AddToBackpack(new MaxxiaScroll()); // Replace with your actual item type
                    player.SendMessage("In gratitude for your interest, take this rune as a reward.");
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("What do you know about ancient runes?",
            player => true,
            player =>
            {
                DialogueModule runesModule = new DialogueModule("The ancient runes are symbols of lost magic, each holding power and meaning. They can unlock hidden abilities if deciphered correctly. I have studied them for many years.");
                runesModule.AddOption("Can you teach me about deciphering them?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule teachRunes = new DialogueModule("To decipher runes, you must first attune yourself to their energy. Focus your mind and let the symbols guide you. Practice with the symbols, and you may unlock their secrets.");
                        pl.SendGump(new DialogueGump(pl, teachRunes));
                    });
                player.SendGump(new DialogueGump(player, runesModule));
            });

        greeting.AddOption("What can you tell me about balance?",
            player => true,
            player =>
            {
                DialogueModule balanceModule = new DialogueModule("Balance is the essence of existence. It is the harmony between light and dark, joy and sorrow. One cannot exist without the other. Seek balance in all aspects of your life.");
                player.SendGump(new DialogueGump(player, balanceModule));
            });

        greeting.AddOption("Can you tell me about the prophecies?",
            player => true,
            player =>
            {
                DialogueModule propheciesModule = new DialogueModule("The prophecies speak of events yet to unfold, of heroes who will rise and fall. They are written in the stars and the hearts of the wise. Seek the truth within yourself, for you may be part of the unfolding tale.");
                propheciesModule.AddOption("How can I discover my role in the prophecies?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule discoverRole = new DialogueModule("To discover your role, you must embark on a journey of self-discovery. Listen to the whispers of your heart and the guidance of the universe. The answers lie within you.");
                        pl.SendGump(new DialogueGump(pl, discoverRole));
                    });
                player.SendGump(new DialogueGump(player, propheciesModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Farewell, traveler. May your path be illuminated.");
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
