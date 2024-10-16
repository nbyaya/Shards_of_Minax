using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class MiraTheMagnificent : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MiraTheMagnificent() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mira the Magnificent";
        Body = 0x191; // Human female body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Stats
        SetStr(50);
        SetDex(70);
        SetInt(70);
        SetHits(65);

        // Appearance
        AddItem(new FancyDress() { Hue = 2126 });
        AddItem(new Sandals() { Hue = 2126 });
        AddItem(new GoldNecklace() { Name = "Mira's Necklace" });
        AddItem(new GoldBracelet() { Name = "Mira's Bracelet" });

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime to a past time
    }

    public MiraTheMagnificent(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Oh, darling! I am Mira the Magnificent. How may I enchant you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Mira the Magnificent, darling. My job is to make the world a more charming place. Isn't that magnificent?");
                aboutModule.AddOption("What challenges have you faced?",
                    p => true,
                    p =>
                    {
                        DialogueModule challengesModule = new DialogueModule("Ah, the challenges I've faced! From treacherous terrains to cunning adversaries, every victory has only added to my magnificent reputation.");
                        challengesModule.AddOption("That sounds difficult.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, challengesModule));
                    });
                aboutModule.AddOption("What makes you elegant?",
                    p => true,
                    p =>
                    {
                        DialogueModule eleganceModule = new DialogueModule("True elegance is not just in appearance, but in character and actions. And if you prove yourself, perhaps I might reward you with a token of my appreciation.");
                        eleganceModule.AddOption("I would like a reward.",
                            pl => CanReward(pl),
                            pl =>
                            {
                                if (GiveReward(pl))
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                }
                                else
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                }
                            });
                        eleganceModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, eleganceModule));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What do you think of battles?",
            player => true,
            player =>
            {
                DialogueModule battlesModule = new DialogueModule("Oh, you're interested in my 'battles'? How amusing! Do you even know what true battles are, darling?");
                battlesModule.AddOption("Tell me more.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule battleDetailsModule = new DialogueModule("True battles are fought with wit and charm, not just swords. I've faced foes that sought to diminish my brilliance, but each time, I emerged victorious.");
                        battleDetailsModule.AddOption("How do you prepare for battles?",
                            p => true,
                            p =>
                            {
                                DialogueModule prepModule = new DialogueModule("Preparation is key, darling! I surround myself with beauty and positivity. It keeps my spirits high and my mind sharp.");
                                prepModule.AddOption("That sounds wise.",
                                    plq => true,
                                    plq =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, prepModule));
                            });
                        battleDetailsModule.AddOption("What was your greatest victory?",
                            p => true,
                            p =>
                            {
                                DialogueModule victoryModule = new DialogueModule("Ah, my greatest victory was defeating the infamous Lord Gloom. With a clever trick and a captivating dance, I turned his own minions against him!");
                                victoryModule.AddOption("You must tell me more about it!",
                                    plw => true,
                                    plw =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, victoryModule));
                            });
                        pl.SendGump(new DialogueGump(pl, battleDetailsModule));
                    });
                battlesModule.AddOption("I'm not interested in battles.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("Do you have any rewards?",
            player => true,
            player =>
            {
                if (GiveReward(player))
                {
                    player.SendMessage("Mira smiles and hands you a token of appreciation.");
                }
                else
                {
                    player.SendMessage("Mira says, 'I have no reward right now. Please return later.'");
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("What is your opinion on charm?",
            player => true,
            player =>
            {
                DialogueModule charmModule = new DialogueModule("Charm is the key to unlocking many doors, darling. It's not merely about appearance; it’s about the aura you project.");
                charmModule.AddOption("Can charm be learned?",
                    p => true,
                    p =>
                    {
                        DialogueModule learnCharmModule = new DialogueModule("Absolutely! It begins with self-confidence. Embrace your uniqueness, and others will be drawn to your light.");
                        learnCharmModule.AddOption("That’s inspiring!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, learnCharmModule));
                    });
                charmModule.AddOption("I’ve never considered that.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, charmModule));
            });

        greeting.AddOption("What trials have you endured?",
            player => true,
            player =>
            {
                DialogueModule trialsModule = new DialogueModule("My trials were not just physical, darling, but mental and emotional too. Each challenge shaped me into the person I am today.");
                trialsModule.AddOption("What was the hardest trial?",
                    p => true,
                    p =>
                    {
                        DialogueModule hardestTrialModule = new DialogueModule("The hardest trial was when I had to face my own insecurities. It was a battle against myself, and I had to emerge victorious to find my true self.");
                        hardestTrialModule.AddOption("That sounds intense.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, hardestTrialModule));
                    });
                trialsModule.AddOption("How did you overcome them?",
                    p => true,
                    p =>
                    {
                        DialogueModule overcomeModule = new DialogueModule("I surrounded myself with positive influences and always reminded myself of my worth. Remember, darling, your greatest ally is your own mind.");
                        overcomeModule.AddOption("Wise words.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, overcomeModule));
                    });
                player.SendGump(new DialogueGump(player, trialsModule));
            });

        greeting.AddOption("Do you believe in fate?",
            player => true,
            player =>
            {
                DialogueModule fateModule = new DialogueModule("Fate is a dance we all participate in. While we can guide our steps, some things are simply meant to be.");
                fateModule.AddOption("So we have no control?",
                    p => true,
                    p =>
                    {
                        DialogueModule controlModule = new DialogueModule("Not entirely, darling. While fate may weave the tapestry, we choose the colors and patterns. It’s a delicate balance.");
                        controlModule.AddOption("That’s a beautiful perspective.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, controlModule));
                    });
                fateModule.AddOption("Interesting thoughts.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, fateModule));
            });

        return greeting;
    }

    private bool CanReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return (DateTime.UtcNow - lastRewardTime) >= cooldown;
    }

    private bool GiveReward(PlayerMobile player)
    {
        if (CanReward(player))
        {
            player.AddToBackpack(new BagOfJuice()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
            return true;
        }
        return false;
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
