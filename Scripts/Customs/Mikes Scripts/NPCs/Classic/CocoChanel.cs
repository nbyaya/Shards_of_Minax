using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CocoChanel : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CocoChanel() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Coco Chanel";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(80);
        SetInt(100);

        SetHits(60);

        // Appearance
        AddItem(new FancyDress() { Hue = 1109 });
        AddItem(new FancyShirt() { Hue = 1109 });
        AddItem(new Skirt() { Hue = 1109 });
        AddItem(new Sandals() { Hue = 1109 });
        AddItem(new Dagger() { Name = "Coco's Dagger" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public CocoChanel(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Bonjour! I am Coco Chanel, a purveyor of style and secrets. How may I assist you today?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("My health? Ah, my dear, health is like fashion; it fluctuates.");
                healthModule.AddOption("Tell me more about fluctuations.",
                    p => true,
                    p =>
                    {
                        DialogueModule fluctuateModule = new DialogueModule("Just as the seasons change, so does our health. It's all a cycle, much like the moon's phases. Speaking of which, the moon has been peculiar lately, don't you think?");
                        fluctuateModule.AddOption("What about the moon?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule moonModule = new DialogueModule("The moon indeed holds many mysteries. Its glow affects more than just the tides. Some even say it holds the power to reveal hidden truths.");
                                moonModule.AddOption("Thank you for the insight.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, moonModule));
                            });
                        fluctuateModule.AddOption("I'll keep that in mind.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, fluctuateModule));
                    });
                healthModule.AddOption("I see, thank you.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job, you ask? I am the weaver of destinies, the designer of dreams. But more specifically, I craft exquisite garments, even in these challenging times.");
                jobModule.AddOption("Why is it challenging to make designer fashions here?",
                    p => true,
                    p =>
                    {
                        DialogueModule challengeModule = new DialogueModule("Ah, my dear, you see, in this medieval land, creating designer fashion is no easy task. There are many challenges one must overcome.");
                        challengeModule.AddOption("What kind of challenges?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule detailsModule = new DialogueModule("First, there is the matter of materials. Fine silk, exquisite dyes, and precious metals for embellishments—these are all scarce. Gathering them requires both wealth and a network of reliable contacts, something most do not possess.");
                                detailsModule.AddOption("Tell me more about acquiring materials.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule materialsModule = new DialogueModule("Acquiring the finest materials involves long journeys, often into dangerous territories. For instance, the silk we use must be imported from the distant Eastern lands. Merchants brave treacherous paths, marauding bandits, and the elements just to bring us a few yards of fabric.");
                                        materialsModule.AddOption("Why is silk so important?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule silkModule = new DialogueModule("Silk is the epitome of elegance, flowing with a grace that no other fabric can match. It shimmers under the candlelight and drapes the body in the most flattering way. No true designer garment would be complete without it, but its rarity makes it incredibly valuable.");
                                                silkModule.AddOption("That sounds like a lot of effort.",
                                                    plaab => true,
                                                    plaab =>
                                                    {
                                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, silkModule));
                                            });
                                        materialsModule.AddOption("What about dyes?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule dyeModule = new DialogueModule("Ah, dyes! The colors of a garment define its impact. To create a striking red, we need the crushed bodies of cochineal insects, which must be collected by hand. For a royal blue, we use lapis lazuli, a rare gemstone that must be ground into powder. Each color has a story, and each is a challenge to obtain.");
                                                dyeModule.AddOption("It must take a lot of work to create a single garment.",
                                                    plaab => true,
                                                    plaab =>
                                                    {
                                                        DialogueModule workModule = new DialogueModule("Indeed, it does. Each garment is a labor of love, requiring countless hours of meticulous work. From spinning thread to weaving fabric, to dyeing, cutting, and sewing—it is a process that can take weeks, if not months. And yet, the final product is worth every moment.");
                                                        workModule.AddOption("How do you keep up with the demand?",
                                                            plaac => true,
                                                            plaac =>
                                                            {
                                                                DialogueModule demandModule = new DialogueModule("Keeping up with demand is impossible, which is why each piece is unique. I create for those who understand the value of true craftsmanship, not for the masses. My clients are those who appreciate the rarity and the story behind every stitch.");
                                                                demandModule.AddOption("Thank you for sharing.",
                                                                    plaad => true,
                                                                    plaad =>
                                                                    {
                                                                        plaad.SendGump(new DialogueGump(plaad, CreateGreetingModule()));
                                                                    });
                                                                plaac.SendGump(new DialogueGump(plaac, demandModule));
                                                            });
                                                        plaab.SendGump(new DialogueGump(plaab, workModule));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, dyeModule));
                                            });
                                        materialsModule.AddOption("Thank you, I understand now.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, materialsModule));
                                    });
                                detailsModule.AddOption("It sounds very difficult indeed.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, detailsModule));
                            });
                        challengeModule.AddOption("That must be why your creations are so unique.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule uniqueModule = new DialogueModule("Precisely. Each garment tells a story of perseverance and dedication. In a world where everything is fleeting, true style endures because of the immense effort behind it.");
                                uniqueModule.AddOption("Thank you for the insight.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, uniqueModule));
                            });
                        p.SendGump(new DialogueGump(p, challengeModule));
                    });
                jobModule.AddOption("Interesting, thank you.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Dreams are fragments of our deepest desires and fears. If you can decipher the colors of your dreams, I might reward you with a token of appreciation.");
                    rewardModule.AddOption("I would love a token.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow;
                            p.SendMessage("Coco gives you a Maxxia Scroll.");
                        });
                    rewardModule.AddOption("Maybe another time.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Coco nods elegantly as you take your leave.");
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