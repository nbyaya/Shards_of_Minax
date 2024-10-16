using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class SlamsonTheBold : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SlamsonTheBold() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Slamson the Bold";
        Body = 0x190; // Human male body

        // Stats
        SetStr(105);
        SetDex(70);
        SetInt(70);
        SetHits(105);

        // Appearance
        AddItem(new BodySash() { Name = "Championship Sash" });
        AddItem(new Skirt() { Hue = 1154 });
        AddItem(new Boots() { Hue = 1175 });
        AddItem(new LeatherGloves() { Name = "Slamson's Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public SlamsonTheBold(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Ah, greetings! I am Slamson the Bold, the champion of the wrestling ring!");

        greeting.AddOption("Tell me about your wrestling moves.",
            player => true,
            player =>
            {
                DialogueModule movesModule = new DialogueModule("I have several signature moves that leave my opponents in awe! Would you like to hear about my favorite?");
                movesModule.AddOption("Yes, what’s your favorite move?",
                    pl => true,
                    pl => DescribeFavoriteMove(pl));
                movesModule.AddOption("Tell me about all your moves.",
                    pl => true,
                    pl => DescribeAllMoves(pl));
                player.SendGump(new DialogueGump(player, movesModule));
            });

        greeting.AddOption("What about your training?",
            player => true,
            player =>
            {
                DialogueModule trainingModule = new DialogueModule("Training for the ring is a spiritual journey, where one learns to sacrifice and to uphold virtues. Every scar is a lesson in humility.");
                trainingModule.AddOption("That sounds intense. How do you prepare?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("I train both body and mind, focusing on strength, technique, and mental fortitude."))));
                player.SendGump(new DialogueGump(player, trainingModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => CanReceiveReward(),
            player =>
            {
                GiveReward(player);
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return greeting;
    }

    private void DescribeFavoriteMove(PlayerMobile player)
    {
        DialogueModule favoriteMoveModule = new DialogueModule("My favorite move is the 'Slamson Slam'! I lift my opponent high above my head and then slam them down with all my might. It’s a crowd favorite!");
        favoriteMoveModule.AddOption("How do you perform the Slamson Slam?",
            pl => true,
            pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("It requires strength and perfect timing. I must gauge my opponent's weight and readiness to ensure a successful execution."))));
        favoriteMoveModule.AddOption("What about your other moves?",
            pl => true,
            pl => DescribeAllMoves(pl));
        player.SendGump(new DialogueGump(player, favoriteMoveModule));
    }

    private void DescribeAllMoves(PlayerMobile player)
    {
        DialogueModule allMovesModule = new DialogueModule("I have a repertoire of moves, each with its own flair. Here are a few:");

        allMovesModule.AddOption("Tell me about the 'Flying Tackle'.",
            pl => true,
            pl =>
            {
                DialogueModule flyingTackleModule = new DialogueModule("The 'Flying Tackle' is a move where I leap off the ropes and tackle my opponent mid-air! It catches them off guard!");
                flyingTackleModule.AddOption("Sounds risky! Have you ever missed?",
                    plq => true,
                    plq => pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! But it’s the thrill of the risk that makes it exhilarating!"))));
                player.SendGump(new DialogueGump(player, flyingTackleModule));
            });

        allMovesModule.AddOption("What about the 'Power Bomb'?",
            pl => true,
            pl =>
            {
                DialogueModule powerBombModule = new DialogueModule("The 'Power Bomb' is one of my most powerful moves. I lift my opponent and slam them onto their back, causing a stunning impact!");
                powerBombModule.AddOption("How do you decide when to use it?",
                    plw => true,
                    plw => pl.SendGump(new DialogueGump(pl, new DialogueModule("It's all about the flow of the match. Timing is everything!"))));
                player.SendGump(new DialogueGump(player, powerBombModule));
            });

        allMovesModule.AddOption("What’s the 'Slamson Stretch'?",
            pl => true,
            pl =>
            {
                DialogueModule stretchModule = new DialogueModule("The 'Slamson Stretch' is a submission move where I put my opponent in a tight hold, testing their endurance. It's both a physical and mental challenge!");
                stretchModule.AddOption("Have you ever won with that move?",
                    ple => true,
                    ple => pl.SendGump(new DialogueGump(pl, new DialogueModule("Yes! It's a great way to force an opponent to tap out when they're exhausted."))));
                player.SendGump(new DialogueGump(player, stretchModule));
            });

        allMovesModule.AddOption("Tell me more about your experiences in the ring.",
            pl => true,
            pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Every match is a story, filled with challenges and triumphs. I have faced many fierce opponents!"))));
        player.SendGump(new DialogueGump(player, allMovesModule));
    }

    private bool CanReceiveReward()
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private void GiveReward(PlayerMobile player)
    {
        Say("Ah, the warrior's brew! Here, have some!");
        player.AddToBackpack(new PetSlotDeed()); // Give the reward
        lastRewardTime = DateTime.UtcNow; // Update the timestamp
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
