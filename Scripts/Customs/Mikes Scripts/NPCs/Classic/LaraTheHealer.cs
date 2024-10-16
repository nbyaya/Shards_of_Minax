using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lara the Healer")]
public class LaraTheHealer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LaraTheHealer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lara the Healer";
        Body = 0x191; // Human female body

        // Stats
        SetStr(100);
        SetDex(70);
        SetInt(80);
        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 1165 }); // Robe with hue 1165
        AddItem(new Sandals() { Hue = 0 }); // Sandals with hue 0
        AddItem(new QuarterStaff() { Name = "Lara's Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lara the Healer. How may I assist you today?");

        greeting.AddOption("What is your name?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I am Lara the Healer."))));

        greeting.AddOption("How is your health?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I am in good health, thank you for asking."))));

        greeting.AddOption("What do you do?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I heal those in need and guide souls seeking wisdom."))));

        greeting.AddOption("Tell me about wisdom.",
            player => true,
            player => 
        {
            DialogueModule wisdomModule = new DialogueModule("The body can be healed with potions and magic, but the soul requires understanding and empathy. Do you seek wisdom?");
            wisdomModule.AddOption("Yes, I do.",
                pl => true,
                pl => 
            {
                DialogueModule seekWisdomModule = new DialogueModule("Then listen closely: The strongest warriors are those who fight not only with their weapons but with their hearts. What do you wish to know about wisdom?");
                seekWisdomModule.AddOption("What is true wisdom?",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("True wisdom comes from experience and compassion. It is the ability to see the world through others' eyes."))));
                seekWisdomModule.AddOption("How can I gain wisdom?",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Seek knowledge relentlessly, and do not shy away from the lessons hidden in hardship."))));
                seekWisdomModule.AddOption("Maybe another time.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                pl.SendGump(new DialogueGump(pl, seekWisdomModule));
            });
            wisdomModule.AddOption("No, I have other questions.",
                pl => true,
                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
            player.SendGump(new DialogueGump(player, wisdomModule));
        });

        greeting.AddOption("What is your philosophy on healing?",
            player => true,
            player => 
        {
            DialogueModule healingPhilosophyModule = new DialogueModule("Healing is more than mending wounds. It's about understanding the root of the pain, whether physical or emotional. I often use herbs in my practice.");
            healingPhilosophyModule.AddOption("What kinds of herbs do you use?",
                p => true,
                p => 
            {
                DialogueModule herbModule = new DialogueModule("I use various herbs like Healing Herbs, Ginseng, and Nightshade. Each has unique properties.");
                herbModule.AddOption("Where can I find these herbs?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Healing Herbs can often be found in grassy meadows, Ginseng in the forest, and Nightshade near darkened areas."))));
                herbModule.AddOption("What about rare herbs?",
                    pl => true,
                    pl => 
                {
                    DialogueModule rareHerbsModule = new DialogueModule("Rare herbs like Dragon's Blood and Phoenix Feather are extremely powerful but also very difficult to find.");
                    rareHerbsModule.AddOption("How can I obtain them?",
                        pla => true,
                        pla => pla.SendGump(new DialogueGump(pla, new DialogueModule("Dragon's Blood is guarded by fierce creatures, and Phoenix Feathers can only be found where the Phoenix resides."))));
                    rareHerbsModule.AddOption("That sounds dangerous.",
                        pla => true,
                        pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                    pl.SendGump(new DialogueGump(pl, rareHerbsModule));
                });
                p.SendGump(new DialogueGump(p, herbModule));
            });
            healingPhilosophyModule.AddOption("Can you teach me about healing?",
                pl => true,
                pl => 
            {
                DialogueModule teachHealingModule = new DialogueModule("Of course! Healing requires patience and practice. Would you like to learn about potions or the art of empathy?");
                teachHealingModule.AddOption("Tell me about potions.",
                    p => true,
                    p => 
                {
                    DialogueModule potionModule = new DialogueModule("Potions are crafted using specific ingredients combined with care. Each potion serves different purposes.");
                    potionModule.AddOption("What kinds of potions can I create?",
                        plq => true,
                        plq => pl.SendGump(new DialogueGump(pl, new DialogueModule("You can create healing potions, stamina potions, and even potions that enhance strength."))));
                    potionModule.AddOption("I want to learn about empathy.",
                        plw => true,
                        plw => pl.SendGump(new DialogueGump(pl, new DialogueModule("Empathy is the ability to understand and share the feelings of others. It requires active listening and a kind heart."))));
                    p.SendGump(new DialogueGump(p, potionModule));
                });
                teachHealingModule.AddOption("Maybe another time.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, teachHealingModule));
            });
            player.SendGump(new DialogueGump(player, healingPhilosophyModule));
        });

        greeting.AddOption("Can you give me a reward for helping others?",
            player => CanReceiveReward(player),
            player =>
        {
            if (lastRewardTime.AddMinutes(10) > DateTime.UtcNow)
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
            }
            else
            {
                player.AddToBackpack(new MaxxiaScroll());
                lastRewardTime = DateTime.UtcNow;
                player.SendGump(new DialogueGump(player, new DialogueModule("For your dedication to understanding, here is a token of my appreciation.")));
            }
        });

        return greeting;
    }

    private bool CanReceiveReward(PlayerMobile player)
    {
        return DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10);
    }

    public LaraTheHealer(Serial serial) : base(serial) { }

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
