using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BabaYaga : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BabaYaga() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Baba Yaga";
        Body = 0x191; // Human female body

        // Stats
        SetStr(85);
        SetDex(55);
        SetInt(105);

        SetHits(65);

        // Appearance
        AddItem(new Skirt(1130));
        AddItem(new Shirt(1130));
        AddItem(new ThighBoots(1130));
        AddItem(new WizardsHat(1130));
        AddItem(new Spellbook() { Name = "Yaga's Book" });

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
        DialogueModule greeting = new DialogueModule("I am Baba Yaga, the all-knowing witch! Speak, mortal, and perhaps I will share my wisdom with you.");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Baba Yaga, feared and revered! Some call me a witch, others a healer. What do you seek from me?");
                identityModule.AddOption("Tell me about your job.",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("My job? Ha! I brew potions, cast spells, and meddle in the affairs of mortals. It's a fulfilling existence, though not without its... darker side.");
                        jobModule.AddOption("What do you mean by darker side?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule darkSideModule = new DialogueModule("Oh, curious one, you wish to know of my darker arts? I twist and bend the wills of the foolish, speak with spirits of the damned, and concoct potions that can twist flesh and soul alike. The screams of those who trespassed against me echo through the night.");
                                darkSideModule.AddOption("Tell me more about these spirits.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule spiritModule = new DialogueModule("The spirits are restless, bound by their own regrets and sins. I call them forth from the shadowed veil, they whisper secrets in exchange for fleeting moments of respite. Some are the spirits of lost children, others of those who dared to betray me. Their voices are a symphony of despair.");
                                        spiritModule.AddOption("How do you control them?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule controlModule = new DialogueModule("Control, you say? It is not control as you would think. It is a pact. They serve me because they fear an eternity without purpose. I hold their regrets in my grasp, and with them, I command their loyalty. But beware, for to hear their cries for too long is to invite madness.");
                                                controlModule.AddOption("This is terrifying...",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, controlModule));
                                            });
                                        spiritModule.AddOption("I should not meddle with the dead.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, spiritModule));
                                    });
                                darkSideModule.AddOption("What do you do with those who cross you?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule revengeModule = new DialogueModule("Those who cross me are given a choice: become a servant in death or suffer eternally. I have turned men into beasts, their minds shattered, their bodies twisted to reflect their true, corrupted souls. There are wolves that roam the forests near my hut—wolves that were once men, now bound to my will.");
                                        revengeModule.AddOption("That is horrifying.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        revengeModule.AddOption("Can you teach me such magic?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule teachModule = new DialogueModule("Teach you? Hah! The path you wish to tread is one of torment and sacrifice. To twist the will of another requires more than mere words—it requires your own spirit to be steeped in darkness. Are you willing to make that sacrifice?");
                                                teachModule.AddOption("I am willing.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule sacrificeModule = new DialogueModule("Very well. The first step is to sever your ties with the light. You must bathe in the blood of a creature that still clings to hope—a lamb, a dove, something innocent. Only then can you begin to learn the spells that bind.");
                                                        sacrificeModule.AddOption("I will do what is necessary.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendMessage("Baba Yaga grins, her eyes glinting with malice. 'Return when the deed is done.'");
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        sacrificeModule.AddOption("I cannot do this.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, sacrificeModule));
                                                    });
                                                teachModule.AddOption("No, I cannot do this.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, teachModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, revengeModule));
                                    });
                                darkSideModule.AddOption("I do not wish to know more.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, darkSideModule));
                            });
                        jobModule.AddOption("Maybe later.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                identityModule.AddOption("Tell me about your wisdom.",
                    p => true,
                    p =>
                    {
                        DialogueModule wisdomModule = new DialogueModule("True wisdom comes from a humble heart. Are you humble enough to learn?");
                        wisdomModule.AddOption("Yes, I am humble.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule humbleModule = new DialogueModule("Ha! Your words amuse me, mortal. Few possess the wisdom they claim, but I shall indulge you for now. My wisdom is not for the faint-hearted. It is forged from the cries of the lost and tempered in the darkness of the human soul.");
                                humbleModule.AddOption("What do you mean by darkness?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule darknessModule = new DialogueModule("The darkness I speak of is within all of us. It is the shadow that whispers doubts, the hunger that drives one to betray, to kill. I have embraced my darkness, listened to it, and allowed it to guide my hand. I know the fears of mortals because I have lived them, and I have fed on them.");
                                        darknessModule.AddOption("How do you feed on fear?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule fearModule = new DialogueModule("Fear is an energy, a force as tangible as fire or wind. When mortals quake before me, their fear seeps into the air. I breathe it in, I consume it. It sustains me, makes me stronger. I have even trapped fear into physical forms—crystals that pulse with the terror of those who dared cross my path.");
                                                fearModule.AddOption("Can I see one of these crystals?",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("Baba Yaga pulls out a dark crystal, its core swirling with an eerie, faintly glowing mist. 'Behold, the essence of pure fear.'");
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                fearModule.AddOption("No, I do not wish to see it.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, fearModule));
                                            });
                                        darknessModule.AddOption("I do not wish to hear any more.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, darknessModule));
                                    });
                                humbleModule.AddOption("Thank you, Baba Yaga.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, humbleModule));
                            });
                        wisdomModule.AddOption("Maybe not.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, wisdomModule));
                    });
                identityModule.AddOption("Goodbye, Baba Yaga.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Baba Yaga cackles as you turn away.");
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Tell me about your potions.",
            player => true,
            player =>
            {
                DialogueModule potionInfoModule = new DialogueModule("Ah, my potions. They are the results of centuries of practice and exploration. Do you wish to know more about their ingredients?");
                potionInfoModule.AddOption("Yes, tell me about the ingredients.",
                    p => true,
                    p =>
                    {
                        DialogueModule ingredientModule = new DialogueModule("The ingredients I use are often rare and dangerous. Ever heard of the Bloodroot Flower?");
                        ingredientModule.AddOption("What is the Bloodroot Flower?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule bloodrootModule = new DialogueModule("The Bloodroot Flower is a rare blossom that only blooms under the blood moon. Many have sought it, few have found it. Its petals can induce visions, or nightmares, depending on the intent of the brewer.");
                                bloodrootModule.AddOption("How do you use the petals?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule petalsModule = new DialogueModule("The petals, when crushed and mixed with the tears of the sorrowful, create a potion that allows one to see beyond the veil of death. It is not without risks—those who gaze too long may find themselves unable to return, their spirits lost forever in the void.");
                                        petalsModule.AddOption("That is dangerous magic.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        petalsModule.AddOption("I wish to try it.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendMessage("Baba Yaga smiles wickedly. 'Very well, bring me the tears of someone who has lost everything, and I shall brew it for you.'");
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, petalsModule));
                                    });
                                bloodrootModule.AddOption("I do not wish to know more.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, bloodrootModule));
                            });
                        ingredientModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, ingredientModule));
                    });
                potionInfoModule.AddOption("Not right now.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, potionInfoModule));
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
                    DialogueModule rewardModule = new DialogueModule("You have impressed me, mortal. Here, take this, and may it serve you well.");
                    rewardModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye, Baba Yaga.",
            player => true,
            player =>
            {
                player.SendMessage("Baba Yaga cackles as you turn away.");
            });

        return greeting;
    }

    public BabaYaga(Serial serial) : base(serial) { }

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