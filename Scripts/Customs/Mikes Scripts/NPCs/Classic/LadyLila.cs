using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Lady Lila")]
public class LadyLila : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LadyLila() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lady Lila";
        Body = 0x191; // Human female body

        // Stats
        SetStr(60);
        SetDex(60);
        SetInt(80);
        SetHits(50);

        // Appearance
        AddItem(new FancyDress() { Hue = 2128 }); // Dress
        AddItem(new Boots() { Hue = 2128 }); // Boots
        AddItem(new Cap() { Hue = 2128 }); // Bonnet
        AddItem(new MortarPestle() { Name = "Lila's Mortar and Pestle" }); // Mortar and Pestle

        Hue = Utility.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime
    }

    public LadyLila(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Lady Lila, a forsaken soul bound to this realm by a cruel curse. How may I assist you, traveler?");

        greeting.AddOption("Tell me about your curse.",
            player => true,
            player =>
            {
                DialogueModule curseModule = new DialogueModule("My curse was cast upon me by a rival witch, filled with jealousy. She used a dark amulet to seal my fate. Find that amulet, and you might be able to help me.");
                curseModule.AddOption("What is the amulet?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateRelicModule()));
                    });
                curseModule.AddOption("Can you tell me more about the witch?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule witchModule = new DialogueModule("The witch's name was Morgath. She was once a friend turned foe, consumed by envy over my family's fortune. She used dark magic to twist my destiny.");
                        witchModule.AddOption("What happened between you two?",
                            plq => true,
                            plq =>
                            {
                                witchModule.AddOption("Why did she envy you?",
                                    pl2 => true,
                                    pl2 =>
                                    {
                                        pl2.SendGump(new DialogueGump(pl2, new DialogueModule("My family was prosperous, and I was to inherit a great legacy. Morgath sought that power for herself.")));
                                    });
                                pl.SendGump(new DialogueGump(pl, witchModule));
                            });
                        pl.SendGump(new DialogueGump(pl, witchModule));
                    });
                player.SendGump(new DialogueGump(player, curseModule));
            });

        greeting.AddOption("What do you serve?",
            player => true,
            player =>
            {
                DialogueModule serviceModule = new DialogueModule("I serve the spirits that reside here, doing their bidding and longing for release. They whisper secrets of the past.");
                serviceModule.AddOption("What secrets do they share?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("They speak of lost treasures and forgotten lands. Some say there are hidden relics that can grant unimaginable power.")));
                    });
                player.SendGump(new DialogueGump(player, serviceModule));
            });

        greeting.AddOption("Can you tell me about your kingdom?",
            player => true,
            player =>
            {
                DialogueModule kingdomModule = new DialogueModule("The kingdom I hailed from was known as Eldoria. It was a place of beauty and wonder, now buried beneath the sands of time.");
                kingdomModule.AddOption("What was Eldoria like?",
                    pl => true,
                    pl =>
                    {
                        kingdomModule.AddOption("What kind of beauty?",
                            pl2 => true,
                            pl2 =>
                            {
                                pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Eldoria was filled with lush forests, sparkling rivers, and majestic mountains. The air was filled with laughter and joy.")));
                            });
                        pl.SendGump(new DialogueGump(pl, kingdomModule));
                    });
                player.SendGump(new DialogueGump(player, kingdomModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
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
                    lastRewardTime = DateTime.UtcNow;
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    player.SendGump(new DialogueGump(player, new DialogueModule("The relic is known as the Tear of Lila. If restored, it can break the strongest of curses. Here is your reward.")));
                }
            });

        greeting.AddOption("What is despair to you?",
            player => true,
            player =>
            {
                DialogueModule despairModule = new DialogueModule("In moments of profound despair, a glimmer of hope sometimes shines through. It's the hope that one day I may be free from this torment.");
                despairModule.AddOption("How can one find hope?",
                    pl => true,
                    pl =>
                    {
                        despairModule.AddOption("Do you believe in fate?",
                            pl2 => true,
                            pl2 =>
                            {
                                pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Fate is a fickle mistress. Sometimes it smiles upon us, other times it leads us into darkness.")));
                            });
                        pl.SendGump(new DialogueGump(pl, despairModule));
                    });
                player.SendGump(new DialogueGump(player, despairModule));
            });

        return greeting;
    }

    private DialogueModule CreateRelicModule()
    {
        DialogueModule relicModule = new DialogueModule("The relic is known as the Tear of Lila. Legend has it that if it's restored to its rightful place, it has the power to break the strongest of curses.");
        relicModule.AddOption("Where can I find it?",
            player => true,
            player =>
            {
                relicModule.AddOption("Is it guarded?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Yes, it is said to be guarded by the spirit of a great warrior. Only the pure of heart may pass unharmed.")));
                    });
                player.SendGump(new DialogueGump(player, relicModule));
            });
        relicModule.AddOption("What will happen if I bring it back?",
            player => true,
            player =>
            {
                relicModule.AddOption("Will you reward me?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("If you bring back the Tear, I will grant you a reward beyond your imagination. But more importantly, you will help free my soul.")));
                    });
                player.SendGump(new DialogueGump(player, relicModule));
            });
        return relicModule;
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
