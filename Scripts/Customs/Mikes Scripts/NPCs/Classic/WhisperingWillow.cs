using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

[CorpseName("the corpse of Whispering Willow")]
public class WhisperingWillow : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public WhisperingWillow() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Whispering Willow";
        Body = 0x191; // Human female body

        // Stats
        SetStr(92);
        SetDex(70);
        SetInt(80);
        SetHits(92);

        // Appearance
        AddItem(new Robe() { Hue = 2968 });
        AddItem(new Sandals() { Hue = 1172 });
        AddItem(new ShepherdsCrook() { Name = "Willow's Whispering Wand" });

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
        DialogueModule greeting = new DialogueModule("I am Whispering Willow, the animal tamer. How can I help you today?");

        greeting.AddOption("How is your health?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("My health is excellent, thanks for asking."))));

        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I have a unique job; I tame and train animals."))));

        greeting.AddOption("What do you think about animals?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Compassion towards animals is a virtue, don't you think?"))));

        greeting.AddOption("Tell me more about your favorite animal.",
            player => true,
            player => 
            {
                DialogueModule favoriteAnimalModule = new DialogueModule("My favorite animal is the majestic wolf. Their loyalty and strength inspire me every day. Would you like to hear more about wolves or perhaps another animal?");
                favoriteAnimalModule.AddOption("Tell me more about wolves.",
                    p => true,
                    p =>
                    {
                        DialogueModule wolfModule = new DialogueModule("Wolves are fascinating creatures! They live in packs, and their social structure is quite intricate. Would you like to know about their hunting strategies or their communication?");
                        wolfModule.AddOption("Tell me about their hunting strategies.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Wolves are cooperative hunters. They use teamwork to take down prey, often surrounding it to corner it. It's a beautiful display of strategy and trust among pack members.")));
                            });
                        wolfModule.AddOption("How do they communicate?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Wolves communicate through howls, growls, and body language. Howling helps them stay in touch over long distances, while growls can signal warnings.")));
                            });
                        wolfModule.AddOption("Maybe another time.",
                            pl => true,
                            pl => pl.SendGump(new DialogueGump(pl, favoriteAnimalModule)));
                        player.SendGump(new DialogueGump(player, wolfModule));
                    });

                favoriteAnimalModule.AddOption("What about other animals?",
                    p => true,
                    p =>
                    {
                        DialogueModule otherAnimalsModule = new DialogueModule("I love many animals! For example, the graceful deer, the playful otters, and the wise old owls. Is there a particular animal you're interested in?");
                        otherAnimalsModule.AddOption("Tell me about deer.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Deer are gentle creatures, often seen grazing peacefully in forests. They are known for their incredible agility and grace. Have you ever seen one up close?")));
                            });
                        otherAnimalsModule.AddOption("What about otters?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Otters are so playful! They love to slide down riverbanks and play with each other. Their social behavior is delightful to observe!")));
                            });
                        otherAnimalsModule.AddOption("I'd like to hear about owls.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Owls are symbols of wisdom. They have excellent night vision and can rotate their heads almost all the way around! What a marvel of nature.")));
                            });
                        otherAnimalsModule.AddOption("Not right now.",
                            pl => true,
                            pl => pl.SendGump(new DialogueGump(pl, favoriteAnimalModule)));
                        player.SendGump(new DialogueGump(player, otherAnimalsModule));
                    });

                player.SendGump(new DialogueGump(player, favoriteAnimalModule));
            });

        greeting.AddOption("How do you care for animals?",
            player => true,
            player =>
            {
                DialogueModule careModule = new DialogueModule("Caring for animals requires patience and understanding. Each animal has its own personality and needs. Would you like to learn about feeding, training, or health care?");
                careModule.AddOption("Tell me about feeding.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Feeding animals varies greatly by species. For example, herbivores need plenty of greens, while carnivores thrive on meat. It's essential to understand their diet.")));
                    });
                careModule.AddOption("What about training?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Training is a blend of discipline and love. It's important to build trust and use positive reinforcement. Each animal learns at its own pace.")));
                    });
                careModule.AddOption("How do you ensure their health?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Regular check-ups with a veterinarian and proper vaccinations are vital. Keeping their living space clean also helps prevent illness.")));
                    });
                player.SendGump(new DialogueGump(player, careModule));
            });

        greeting.AddOption("Do you have any animal companions?",
            player => true,
            player =>
            {
                DialogueModule companionsModule = new DialogueModule("I have a few beloved companions! My most cherished is a wolf named Shadow. Would you like to hear more about him or my other companions?");
                companionsModule.AddOption("Tell me about Shadow.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Shadow is a loyal and protective friend. He follows me everywhere and helps me with my work. His intuition about animals is remarkable.")));
                    });
                companionsModule.AddOption("What about your other companions?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("I also have a mischievous raccoon named Bandit and a wise old owl named Athena. They each have their unique personalities and quirks.")));
                    });
                player.SendGump(new DialogueGump(player, companionsModule));
            });

        greeting.AddOption("What do you think about animal cruelty?",
            player => true,
            player =>
            {
                DialogueModule crueltyModule = new DialogueModule("Animal cruelty is heartbreaking. Every creature deserves love and respect. I work to rescue those in need and promote compassion. Would you like to join me in this cause?");
                crueltyModule.AddOption("How can I help?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("You can help by spreading awareness and volunteering at shelters. Even small acts of kindness can make a difference.")));
                    });
                crueltyModule.AddOption("I can't right now.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, greeting)));
                player.SendGump(new DialogueGump(player, crueltyModule));
            });

        return greeting;
    }

    public WhisperingWillow(Serial serial) : base(serial) { }

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
