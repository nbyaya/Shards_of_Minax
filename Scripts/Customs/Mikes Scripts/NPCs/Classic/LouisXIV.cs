using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Louis XIV")]
public class LouisXIV : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LouisXIV() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Louis XIV";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(60);
        SetInt(100);
        SetHits(85);

        // Appearance
        AddItem(new FancyShirt() { Hue = 1202 });
        AddItem(new LongPants() { Hue = 1141 });
        AddItem(new Boots() { Hue = 1913 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
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
        DialogueModule greeting = new DialogueModule("I am Louis XIV, the Sun King of France! How may I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("I am in the best of health, as befits a king!"))); });

        greeting.AddOption("What is your job?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("My job is to rule France with absolute power!"))); });

        greeting.AddOption("What do you mean by rule?",
            player => true,
            player => {
                var ruleModule = new DialogueModule("True power lies not in the crown, but in the hearts of the people. Do you understand, adventurer?");
                ruleModule.AddOption("Yes, I understand.",
                    pl => true,
                    pl => {
                        var virtueModule = new DialogueModule("Indeed, my reign is a testament to the virtues of leadership and power!");
                        virtueModule.AddOption("What virtues do you uphold?",
                            p => true,
                            p => {
                                var detailModule = new DialogueModule("Virtues such as honor, loyalty, and courage are the pillars upon which I've built my reign. They guide my decisions every day.");
                                detailModule.AddOption("How do you demonstrate loyalty?",
                                    plq => true,
                                    plq => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Loyalty is shown by standing firm with your allies, even in the face of adversity."))); });
                                detailModule.AddOption("And what of courage?",
                                    plw => true,
                                    plw => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Courage is about facing the unknown and making difficult choices for the greater good."))); });
                                detailModule.AddOption("Tell me more about your decisions.",
                                    ple => true,
                                    ple => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Every decision is weighed carefully, considering the welfare of my subjects and the future of France."))); });
                                pl.SendGump(new DialogueGump(pl, detailModule));
                            });
                        pl.SendGump(new DialogueGump(pl, virtueModule));
                    });
                ruleModule.AddOption("No, I do not.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("It is vital to understand the essence of leadership, dear adventurer.")));
                    });
                player.SendGump(new DialogueGump(player, ruleModule));
            });

        greeting.AddOption("Tell me about the Sun King.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("Ah, you've heard of my title, the Sun King. It signifies my radiant presence and the golden age of France under my rule."))); });

        greeting.AddOption("Do you have a reward for me?",
            player => CanReceiveReward(player),
            player => GiveReward(player));

        greeting.AddOption("What about the nobles of your court?",
            player => true,
            player => {
                var noblesModule = new DialogueModule("The nobles are both allies and rivals. They often seek power, and I must be cautious.");
                noblesModule.AddOption("How do you keep them in check?",
                    pl => true,
                    pl => {
                        var controlModule = new DialogueModule("I employ a delicate balance of rewards and consequences. Diplomacy is as crucial as strength.");
                        controlModule.AddOption("Do you ever fear betrayal?",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, new DialogueModule("Fear is a natural instinct, but I choose to trust my allies until they prove otherwise."))); });
                        controlModule.AddOption("What if they succeed?",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, new DialogueModule("Then I must adapt and react swiftly to maintain my position."))); });
                        pl.SendGump(new DialogueGump(pl, controlModule));
                    });
                player.SendGump(new DialogueGump(player, noblesModule));
            });

        return greeting;
    }

    private bool CanReceiveReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private void GiveReward(PlayerMobile player)
    {
        if (!CanReceiveReward(player))
        {
            player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
            return;
        }

        player.SendGump(new DialogueGump(player, new DialogueModule("As a token of my appreciation, accept this small gift from the royal treasury. May it serve you well on your adventures.")));
        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
        lastRewardTime = DateTime.UtcNow; // Update the timestamp
    }

    public LouisXIV(Serial serial) : base(serial) { }

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
