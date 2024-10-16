using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Mistress Mira")]
public class MistressMira : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MistressMira() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mistress Mira";
        Body = 0x191; // Human female body

        // Stats
        SetStr(85);
        SetDex(80);
        SetInt(55);
        SetHits(65);

        // Appearance
        AddItem(new FancyDress() { Hue = 1301 });
        AddItem(new GoldNecklace() { Name = "Mira's Necklace" });
        AddItem(new Boots() { Hue = 1157 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
        SpeechHue = 0; // Default speech hue
    }

    public MistressMira(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Ah, welcome! I am Mistress Mira, a courtesan of exquisite talents. How may I assist you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Mistress Mira, an artist of allure and charm. My life is dedicated to the dance of seduction, providing companionship and solace to those who seek it.");
                aboutModule.AddOption("What is your greatest talent?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule talentModule = new DialogueModule("My greatest talent lies in the art of conversation. A well-placed word can create connections that last a lifetime.");
                        talentModule.AddOption("Can you teach me?",
                            pla => true,
                            pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                        pl.SendGump(new DialogueGump(pl, talentModule));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What is your heart's wound?",
            player => true,
            player =>
            {
                DialogueModule heartModule = new DialogueModule("Why do you care about my health? It's my heart that suffers, a wound left by a knight who chose valor over love.");
                heartModule.AddOption("What happened?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule storyModule = new DialogueModule("He was a beacon of courage and strength, but in his quest for glory, he forgot the promises of the heart. Love is a heavy burden, yet so sweet.");
                        storyModule.AddOption("That sounds painful.",
                            pla => true,
                            pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                        pl.SendGump(new DialogueGump(pl, storyModule));
                    });
                player.SendGump(new DialogueGump(player, heartModule));
            });

        greeting.AddOption("What battles do you face?",
            player => true,
            player =>
            {
                DialogueModule battlesModule = new DialogueModule("Do you think life in the shadows is easy? The battles we fight are not with swords but with the heart's desires and the mind's temptations.");
                battlesModule.AddOption("Tell me more about seduction.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule seductionModule = new DialogueModule("Seduction is an intricate dance, a blend of charm, wit, and the ability to read one's desires. Just as a warrior masters their blade, I have honed my skills in the art of allure.");
                        seductionModule.AddOption("What are your secrets?",
                            pla => true,
                            pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                        pl.SendGump(new DialogueGump(pl, seductionModule));
                    });
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("Can you reward me for pondering?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. For your thoughtful inquiry, please accept this reward.");
                    player.AddToBackpack(new Gold(1000)); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("What is the value of companionship?",
            player => true,
            player =>
            {
                DialogueModule companionshipModule = new DialogueModule("Companionship is priceless. It's a balm for the soul, a momentary escape from the burdens of reality. The whispered secrets and shared laughter are treasures in themselves.");
                companionshipModule.AddOption("How do you find your clients?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule clientModule = new DialogueModule("I attract those who seek something more than mere company. It’s a mutual understanding, an unspoken connection. Every encounter is a story waiting to unfold.");
                        clientModule.AddOption("That sounds enchanting.",
                            pla => true,
                            pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                        pl.SendGump(new DialogueGump(pl, clientModule));
                    });
                player.SendGump(new DialogueGump(player, companionshipModule));
            });

        greeting.AddOption("What do you know about love?",
            player => true,
            player =>
            {
                DialogueModule loveModule = new DialogueModule("Love is a complex tapestry of emotions, woven with threads of joy, sorrow, passion, and pain. It's the greatest adventure one can embark upon, but it can also lead to the deepest heartaches.");
                loveModule.AddOption("Have you loved?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule lovedModule = new DialogueModule("I have loved deeply, and the memories linger like the sweetest melody. Each love has shaped me, leaving echoes in my heart that guide my steps.");
                        lovedModule.AddOption("That is beautiful.",
                            pla => true,
                            pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                        pl.SendGump(new DialogueGump(pl, lovedModule));
                    });
                player.SendGump(new DialogueGump(player, loveModule));
            });

        greeting.AddOption("What do you seek?",
            player => true,
            player =>
            {
                DialogueModule seekModule = new DialogueModule("I seek the fleeting moments of joy and connection, the laughter shared over a fine wine, and the warmth of a kindred spirit beside me.");
                seekModule.AddOption("What is your greatest wish?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule wishModule = new DialogueModule("My greatest wish is to find a love that transcends time, a connection so profound that it becomes a part of my very soul.");
                        wishModule.AddOption("I hope you find it.",
                            pla => true,
                            pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                        pl.SendGump(new DialogueGump(pl, wishModule));
                    });
                player.SendGump(new DialogueGump(player, seekModule));
            });

        greeting.AddOption("What is your perspective on life?",
            player => true,
            player =>
            {
                DialogueModule perspectiveModule = new DialogueModule("Life is a grand stage, and we are all but actors in this play. Each encounter, each moment, is a part of our story, shaping us into who we are meant to become.");
                perspectiveModule.AddOption("Do you enjoy being on this stage?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule enjoymentModule = new DialogueModule("I do, indeed! The thrill of connection and the stories we weave together bring joy to my heart. Each person is a new chapter in my book.");
                        enjoymentModule.AddOption("What stories have you collected?",
                            pla => true,
                            pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                        pl.SendGump(new DialogueGump(pl, enjoymentModule));
                    });
                player.SendGump(new DialogueGump(player, perspectiveModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Mistress Mira smiles warmly and waves goodbye, her eyes sparkling with mystery.");
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
