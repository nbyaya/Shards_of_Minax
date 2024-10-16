using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class SeleneTheNightshade : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SeleneTheNightshade() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Selene the Nightshade";
        Body = 0x191; // Human female body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Stats
        SetStr(80);
        SetDex(130);
        SetInt(60);
        SetHits(90);

        // Appearance
        AddItem(new ThighBoots() { Hue = 1171 });
        AddItem(new LeatherBustierArms() { Hue = 1172 });
        AddItem(new Kryss() { Name = "Selene's Dagger" });

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public SeleneTheNightshade(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Selene the Nightshade, a shadow elemental. What do you want?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("As a shadow elemental, I thrive in darkness. My existence is entwined with secrets and shadows. Do you wish to learn about my... favorite pastimes?");
                aboutModule.AddOption("What are your favorite pastimes?",
                    p => true,
                    p =>
                    {
                        DialogueModule pastimesModule = new DialogueModule("Ah, sweet mortal, I delight in weaving through the shadows, lurking in the darkness, and... hunting. Nothing is more exhilarating than the thrill of the chase.");
                        pastimesModule.AddOption("Who do you hunt?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule huntModule = new DialogueModule("I prefer those who think themselves invulnerable. They are often the most satisfying targets. Like the self-righteous knights, who prattle on about honor and justice.");
                                huntModule.AddOption("Knights? Why them?",
                                    pq => true,
                                    pq =>
                                    {
                                        DialogueModule knightsModule = new DialogueModule("They wear their shining armor like a shield against the shadows. Their arrogance blinds them to the lurking dangers. When I strike, the look of disbelief on their faces is... delightful.");
                                        knightsModule.AddOption("Have you ever failed?",
                                            plw => true,
                                            plw =>
                                            {
                                                pl.SendMessage("Once or twice, perhaps. But I always learn from my mistakes. They add flavor to my endeavors.");
                                            });
                                        p.SendGump(new DialogueGump(p, knightsModule));
                                    });
                                huntModule.AddOption("What about mages?",
                                    ple => true,
                                    ple =>
                                    {
                                        DialogueModule magesModule = new DialogueModule("Ah, mages! They believe their spells can protect them. But magic can be as fickle as the wind. When their incantations falter, they become mere mortals once more.");
                                        magesModule.AddOption("Do you enjoy the hunt?",
                                            pr => true,
                                            pr =>
                                            {
                                                p.SendMessage("The thrill is intoxicating. Each shadow holds a possibility, each flicker of light a chance for ambush. To hunt is to dance with death, and I am a master of the waltz.");
                                            });
                                        pl.SendGump(new DialogueGump(pl, magesModule));
                                    });
                                p.SendGump(new DialogueGump(p, huntModule));
                            });
                        greeting.AddOption("What other creatures do you target?",
                            playert => true,
                            playert =>
                            {
                                DialogueModule otherTargetsModule = new DialogueModule("I relish the hunt of those who dwell in the light. Perhaps a radiant paladin or a naïve healer, believing they can save the world.");
                                otherTargetsModule.AddOption("Why not hunt those in the shadows?",
                                    pl => true,
                                    pl =>
                                    {
                                        pl.SendMessage("The shadows hold no challenge for me. My prey lies in the light, where their hubris makes them vulnerable.");
                                    });
                                p.SendGump(new DialogueGump(p, otherTargetsModule));
                            });
                        p.SendGump(new DialogueGump(p, pastimesModule));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                player.SendMessage("I exist in shadows, orchestrating the dance of death. Each target is a note in my symphony of silence.");
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    player.SendMessage("In every flicker lies an opportunity. Take this.");
                    player.AddToBackpack(new ThrowingAugmentCrystal()); // Replace with the actual item to give
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            });

        greeting.AddOption("What do you know about battles?",
            player => true,
            player =>
            {
                player.SendMessage("Battles? Ah, they are mere distractions. I prefer the subtle art of stealth over the clashing of swords.");
            });

        greeting.AddOption("What do you think of mortals?",
            player => true,
            player =>
            {
                player.SendMessage("Mortals are fascinating, yet tragically unaware of the true darkness that surrounds them. Their fleeting lives are but a candle in the night.");
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
