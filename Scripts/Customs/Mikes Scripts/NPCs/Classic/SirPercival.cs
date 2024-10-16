using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Sir Percival")]
public class SirPercival : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SirPercival() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sir Percival";
        Body = 0x190; // Human male body

        // Stats
        SetStr(160);
        SetDex(70);
        SetInt(30);
        SetHits(120);

        // Appearance
        AddItem(new ChainChest() { Hue = 1153 });
        AddItem(new ChainLegs() { Hue = 1153 });
        AddItem(new PlateHelm() { Hue = 1153 });
        AddItem(new PlateGloves() { Hue = 1153 });
        AddItem(new Cloak() { Hue = 1153 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

	public SirPercival(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Sir Percival, a knight of virtue and a lover of sheep. How may I assist you today?");

        greeting.AddOption("Tell me about your love of sheep.",
            player => true,
            player =>
            {
                DialogueModule sheepLoveModule = new DialogueModule("Ah, sheep! The gentle creatures that bring peace to my heart. They embody tranquility and simplicity. Did you know I once had a sheep named Wooly who was quite the companion?");
                sheepLoveModule.AddOption("What made Wooly special?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule woolySpecialModule = new DialogueModule("Wooly had a unique personality! He would follow me everywhere, even to battlefields. He seemed to sense when I needed comfort, and his soft bleating always brought a smile to my face.");
                        woolySpecialModule.AddOption("What adventures did you have together?",
                            p => true,
                            p =>
                            {
                                DialogueModule adventuresModule = new DialogueModule("Oh, many adventures! One time, we ventured into the Whispering Forest, where we encountered a mischievous pixie. Wooly charmed her with his fluffy cuteness, and she granted us a wish!");
                                adventuresModule.AddOption("What wish did you make?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule wishModule = new DialogueModule("I wished for a field filled with the greenest grass for all sheep to roam freely. Since then, I've dedicated my life to ensuring the safety and happiness of sheep everywhere.");
                                        wishModule.AddOption("That’s noble of you!",
                                            pla => true,
                                            pla =>
                                            {
                                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                            });
                                        p.SendGump(new DialogueGump(p, wishModule));
                                    });
                                adventuresModule.AddOption("Did you ever encounter trouble?",
                                    plw => true,
                                    plw =>
                                    {
                                        DialogueModule troubleModule = new DialogueModule("Indeed! We once faced a hungry wolf! Wooly, in his bravery, distracted the beast long enough for me to scare it away with a loud shout. He truly is a hero in my eyes.");
                                        troubleModule.AddOption("Sheep can be brave?",
                                            pla => true,
                                            pla =>
                                            {
                                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                            });
                                        p.SendGump(new DialogueGump(p, troubleModule));
                                    });
                                p.SendGump(new DialogueGump(p, adventuresModule));
                            });
                        woolySpecialModule.AddOption("What happened to Wooly?",
                            p => true,
                            p =>
                            {
                                DialogueModule woolyEndModule = new DialogueModule("Alas, Wooly grew old and passed away. I buried him in a beautiful meadow where he once grazed, surrounded by blooming flowers. His spirit lives on in my heart and in every sheep I encounter.");
                                woolyEndModule.AddOption("You honor his memory well.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, woolyEndModule));
                            });
                        pl.SendGump(new DialogueGump(pl, woolySpecialModule));
                    });

                sheepLoveModule.AddOption("Why do you love sheep so much?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("Sheep are symbols of peace and gentleness. Their wool provides warmth, and they remind us to appreciate the simpler things in life. Plus, they're always so fluffy!");
                        reasonModule.AddOption("Do you have any sheep now?",
                            p => true,
                            p =>
                            {
                                DialogueModule currentSheepModule = new DialogueModule("Currently, I do not have any sheep. I roam the lands as a knight, but I often visit farms to help with their care. Each sheep I meet brings back memories of Wooly.");
                                currentSheepModule.AddOption("I can help you find a sheep!",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("Oh, how wonderful! If you could help me find a little lamb to raise, I would be ever grateful. They are such delightful companions.");
                                        helpModule.AddOption("Where should I look?",
                                            p1 => true,
                                            p1 =>
                                            {
                                                p1.SendGump(new DialogueGump(p1, CreateGreetingModule()));
                                            });
                                        p.SendGump(new DialogueGump(p, helpModule));
                                    });
                                p.SendGump(new DialogueGump(p, currentSheepModule));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });

                player.SendGump(new DialogueGump(player, sheepLoveModule));
            });

        greeting.AddOption("What do you know about the realm?",
            player => true,
            player =>
            {
                DialogueModule realmModule = new DialogueModule("The realm has been relatively peaceful of late, thanks to the vigilance of its defenders. Yet, dark shadows stir in the North. Beware if you venture there.");
                realmModule.AddOption("Thank you for the warning.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, realmModule));
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
                    player.SendMessage("For those who truly strive to uphold the virtues, rewards are boundless. Take this as a token of appreciation for your dedication.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
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
