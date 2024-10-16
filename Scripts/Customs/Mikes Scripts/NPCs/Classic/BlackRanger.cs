using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Items;

public class BlackRanger : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BlackRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Black Ranger";
        Body = 0x190; // Human male body

        // Stats
        SetStr(115);
        SetDex(100);
        SetInt(60);

        SetHits(85);
        Fame = 0;
        Karma = 0;
        VirtualArmor = 20;

        // Appearance
        AddItem(new PlateLegs() { Hue = 1 });
        AddItem(new PlateChest() { Hue = 1 });
        AddItem(new PlateHelm() { Hue = 1 });
        AddItem(new PlateGloves() { Hue = 1 });
        AddItem(new Boots() { Hue = 1 });
        AddItem(new WarAxe() { Name = "Black Ranger's Axe" });

        // Standard hair information
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public BlackRanger(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am the Black Ranger, guardian of the shadows. What do you seek?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am the Black Ranger, protector of the shadows and keeper of secrets. But long ago, I was something more... I was one of the Power Rangers.");
                identityModule.AddOption("You were a Power Ranger?",
                    p => true,
                    p =>
                    {
                        DialogueModule powerRangerModule = new DialogueModule("Indeed, I fought alongside the legendary team. We defended the world from great evil, using our skills, courage, and the power of our bond as a team.");
                        powerRangerModule.AddOption("Tell me about your time with the Power Rangers.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule timeWithRangersModule = new DialogueModule("We were more than just warriors. We were a family. We trained together, fought together, and supported each other through the hardest of times. There was something about those days that still brings warmth to my heart.");
                                timeWithRangersModule.AddOption("Who were your teammates?",
                                    pm => true,
                                    pm =>
                                    {
                                        DialogueModule teammatesModule = new DialogueModule("There was Jason, the Red Ranger, a true leader. Zack, the playful but brave Black Ranger—my predecessor. Trini, the wise Yellow Ranger, who always inspired me. Kimberly, the compassionate Pink Ranger, and Billy, the brilliant Blue Ranger.");
                                        teammatesModule.AddOption("Tell me more about the Yellow Ranger.",
                                            plr => true,
                                            plr =>
                                            {
                                                DialogueModule yellowRangerModule = new DialogueModule("Trini... she was incredible. Strong, calm, and always had a smile that could light up even the darkest days. I... I had feelings for her, feelings I never expressed. I was always too focused on my duty.");
                                                yellowRangerModule.AddOption("You had a crush on her?",
                                                    plyr => true,
                                                    plyr =>
                                                    {
                                                        DialogueModule crushModule = new DialogueModule("Yes, I did. I suppose it was the way she faced every challenge with grace and courage. She never knew, or maybe she did, but it was never the right time. In the midst of battles, there was little space for matters of the heart.");
                                                        crushModule.AddOption("Did you ever regret not telling her?",
                                                            ply => true,
                                                            ply =>
                                                            {
                                                                DialogueModule regretModule = new DialogueModule("Sometimes, yes. But I also knew that what we did—protecting the world—was bigger than any one of us. I chose to stay silent because I feared it might complicate our bond as a team. Still, there are moments when I wonder what might have been.");
                                                                regretModule.AddOption("That must have been difficult.",
                                                                    pml => true,
                                                                    pml =>
                                                                    {
                                                                        pml.SendGump(new DialogueGump(pml, CreateGreetingModule()));
                                                                    });
                                                                regretModule.AddOption("Would you have done it differently now?",
                                                                    pml => true,
                                                                    pml =>
                                                                    {
                                                                        DialogueModule differentlyModule = new DialogueModule("Perhaps. Life has taught me that moments of connection are fleeting, and we must seize them. But I also understand that we each have our own journey, and perhaps Trini's path was not meant to align with mine in that way.");
                                                                        differentlyModule.AddOption("That's a wise perspective.",
                                                                            pmm => true,
                                                                            pmm =>
                                                                            {
                                                                                pmm.SendGump(new DialogueGump(pmm, CreateGreetingModule()));
                                                                            });
                                                                        pml.SendGump(new DialogueGump(pml, differentlyModule));
                                                                    });
                                                                ply.SendGump(new DialogueGump(ply, regretModule));
                                                            });
                                                        crushModule.AddOption("Thank you for sharing that with me.",
                                                            plyrq => true,
                                                            plyrq =>
                                                            {
                                                                plyr.SendGump(new DialogueGump(plyr, CreateGreetingModule()));
                                                            });
                                                        plr.SendGump(new DialogueGump(plr, crushModule));
                                                    });
                                                yellowRangerModule.AddOption("She sounds remarkable.",
                                                    plrw => true,
                                                    plrw =>
                                                    {
                                                        plr.SendGump(new DialogueGump(plr, CreateGreetingModule()));
                                                    });
                                                pm.SendGump(new DialogueGump(pm, yellowRangerModule));
                                            });
                                        teammatesModule.AddOption("It sounds like you had a close bond.",
                                            pme => true,
                                            pme =>
                                            {
                                                pm.SendGump(new DialogueGump(pm, CreateGreetingModule()));
                                            });
                                        pl.SendGump(new DialogueGump(pl, teammatesModule));
                                    });
                                timeWithRangersModule.AddOption("Those must have been incredible times.",
                                    pr => true,
                                    pr =>
                                    {
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, timeWithRangersModule));
                            });
                        powerRangerModule.AddOption("Did you have a favorite moment?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule favoriteMomentModule = new DialogueModule("There were many. But if I had to choose, it would be the time we finally defeated one of our greatest foes—Rita Repulsa. The sense of triumph, of relief, and seeing my friends smile... it was unforgettable.");
                                favoriteMomentModule.AddOption("Victory well deserved.",
                                    pt => true,
                                    pt =>
                                    {
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, favoriteMomentModule));
                            });
                        p.SendGump(new DialogueGump(p, powerRangerModule));
                    });
                identityModule.AddOption("Farewell, Black Ranger.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("You take your leave from the enigmatic Black Ranger.");
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Do you have any wisdom to share?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("Within the darkness, one must find their own light. Do you seek wisdom or guidance?");
                wisdomModule.AddOption("Yes, I seek wisdom.",
                    p => true,
                    p =>
                    {
                        DialogueModule yesWisdomModule = new DialogueModule("Your response reveals much. Seek the hidden paths, for therein lies true strength.");
                        yesWisdomModule.AddOption("Thank you, Black Ranger.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, yesWisdomModule));
                    });
                wisdomModule.AddOption("No, perhaps another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Tell me about the Whispering Grove.",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no reward to offer right now. Please return later.");
                    cooldownModule.AddOption("I understand, I will return.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule groveModule = new DialogueModule("Ah, the Whispering Grove, a hidden place where trees murmur ancient tales. Seek it out, and you might discover truths long forgotten. Take this as a token of my appreciation.");
                    groveModule.AddOption("Thank you, Black Ranger.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new BanishingRod());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, groveModule));
                }
            });

        greeting.AddOption("Farewell, Black Ranger.",
            player => true,
            player =>
            {
                player.SendMessage("The Black Ranger nods in acknowledgment as you take your leave.");
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