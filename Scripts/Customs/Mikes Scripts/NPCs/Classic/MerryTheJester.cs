using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class MerryTheJester : BaseCreature
{
    private DateTime lastGiftTime;

    [Constructable]
    public MerryTheJester() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Merry the Jester";
        Body = 0x190; // Human male body

        // Stats
        SetStr(50);
        SetDex(50);
        SetInt(50);
        SetHits(50);

        // Appearance
        AddItem(new FancyShirt() { Hue = 2213 });
        AddItem(new FloppyHat() { Hue = 2213 });
        
        // Hair and skin
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastGiftTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("Welcome, dear traveler! I am Merry the Jester, here to brighten your day with humor and riddles!");

        greeting.AddOption("Tell me your name.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

        greeting.AddOption("How are you?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I'm in good spirits, as always! Care to share some of yours?"))));

        greeting.AddOption("What do you do?",
            player => true,
            player => 
            {
                var module = new DialogueModule("My job is to make people laugh and think! I'm a jester, after all. But I also dabble in tales and riddles. Would you like to hear one?");
                module.AddOption("Yes, please!",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateRiddleModule())));
                module.AddOption("No, maybe another time.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, module));
            });

        greeting.AddOption("Tell me a riddle.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateRiddleModule())));

        greeting.AddOption("Make me laugh!",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateJokeModule())));

        greeting.AddOption("What about Merry?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Ah, my name carries stories from towns near and far. Ever heard of the 'Great Jest of Luna'?"))));

        greeting.AddOption("Do you know about potions?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreatePotionModule())));

        greeting.AddOption("Tell me about the Ancient Medallion.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateMedallionModule())));

        greeting.AddOption("Do you have a gift for me?",
            player => CanReceiveGift(),
            player => GiveGift(player));

        return greeting;
    }

    private DialogueModule CreateRiddleModule()
    {
        var riddleModule = new DialogueModule("Here’s one: I speak without a mouth and hear without ears. I have no body, but I come alive with the wind. What am I?");
        riddleModule.AddOption("An echo!",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Well done! You're quite clever!"))));
        riddleModule.AddOption("I don't know.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("It's an echo! Come back for more riddles anytime!"))));
        return riddleModule;
    }

    private DialogueModule CreateJokeModule()
    {
        var jokeModule = new DialogueModule("What did one wall say to the other wall? I'll meet you at the corner!");
        jokeModule.AddOption("That's funny!",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
        jokeModule.AddOption("Do you have more?",
            player => true,
            player =>
            {
                var moreJokesModule = new DialogueModule("Sure! Why don’t scientists trust atoms? Because they make up everything!");
                moreJokesModule.AddOption("That's hilarious!",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                moreJokesModule.AddOption("Tell me another.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateJokeModule())));
                player.SendGump(new DialogueGump(player, moreJokesModule));
            });
        return jokeModule;
    }

    private DialogueModule CreatePotionModule()
    {
        var potionModule = new DialogueModule("Potions can be tricky. Once, a witch gave me a potion that was supposed to make me invisible, but it only made my nose disappear! Ever dealt with witches?");
        potionModule.AddOption("I've dealt with witches.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Ah, then you know their ways! What's your story?"))));
        potionModule.AddOption("No, what's a witch like?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Witches can be unpredictable. There's one named 'Elara' who is known for her peculiar brews. If you ever cross paths, be wary of her concoctions."))));
        return potionModule;
    }

    private DialogueModule CreateMedallionModule()
    {
        var medallionModule = new DialogueModule("The Ancient Medallion is said to hold immense power. Legend says it's hidden in the 'Enchanted Forest'. But, tread with caution, brave adventurer.");
        medallionModule.AddOption("Where is the Enchanted Forest?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("It's a mystical place, filled with wonders and dangers alike. Many who seek it are never seen again. But those who are brave may find great rewards."))));
        medallionModule.AddOption("What will I find there?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Rumor has it that the forest is home to ancient spirits and fearsome beasts. Many have claimed to hear whispers guiding them. Will you follow the whispers?"))));
        return medallionModule;
    }

    private bool CanReceiveGift()
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastGiftTime >= cooldown;
    }

    private void GiveGift(PlayerMobile player)
    {
        if (CanReceiveGift())
        {
            player.SendMessage("Ah, for someone as curious as you, here it is! [Merry hands you a small package]. May it serve you well in your adventures.");
            player.AddToBackpack(new MaxxiaScroll()); // Give the reward
            lastGiftTime = DateTime.UtcNow; // Update the timestamp
        }
        else
        {
            player.SendMessage("I have no reward right now. Please return later.");
        }
    }

    public MerryTheJester(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastGiftTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastGiftTime = reader.ReadDateTime();
    }
}
