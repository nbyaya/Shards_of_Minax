using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class SirRoland : BaseCreature
{
    [Constructable]
    public SirRoland() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sir Roland";
        Body = 0x190; // Human male body

        // Stats
        SetStr(165);
        SetDex(72);
        SetInt(28);
        SetHits(125);

        // Appearance
        AddItem(new PlateChest() { Hue = 1175 });
        AddItem(new PlateLegs() { Hue = 1175 });
        AddItem(new CloseHelm() { Hue = 1175 });
        AddItem(new PlateGloves() { Hue = 1175 });
        AddItem(new Cloak() { Hue = 1175 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
    }

    public SirRoland(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, noble traveler. I am Sir Roland, a humble knight. I have a peculiar passion for turtles! Would you like to hear more about it?");
        
        greeting.AddOption("Tell me about your love for turtles.",
            player => true,
            player => 
            {
                DialogueModule turtleLoveModule = new DialogueModule("Ah, turtles! Majestic creatures that embody patience and wisdom. They remind me of the importance of perseverance. Would you like to know about my favorite species?");
                
                turtleLoveModule.AddOption("What is your favorite species?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule speciesModule = new DialogueModule("My favorite species is the Galápagos tortoise. They can live for over a hundred years and are a symbol of longevity. Have you ever seen one?");
                        
                        speciesModule.AddOption("I have seen one!",
                            plq => true,
                            plq => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Wonderful! They are truly awe-inspiring, aren't they? Their shells are like living armor."))); });
                        
                        speciesModule.AddOption("No, but I've read about them.",
                            plw => true,
                            plw => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Reading about them is almost as good! Their history is fascinating, especially their role in conservation efforts."))); });

                        speciesModule.AddOption("I don't know much about turtles.",
                            ple => true,
                            ple => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, allow me to enlighten you! Turtles have existed for millions of years and play crucial roles in their ecosystems. Would you like to learn more about their habitats?")));
                            });
                        
                        turtleLoveModule.AddOption("What else do you love about turtles?",
                            plr => true,
                            plr => { pl.SendGump(new DialogueGump(pl, new DialogueModule("I admire their tranquility and wisdom. They remind me to take life at a steady pace, enjoying every moment."))); });

                        turtleLoveModule.AddOption("Tell me about turtle conservation.",
                            plt => true,
                            plt => 
                            {
                                DialogueModule conservationModule = new DialogueModule("Turtle conservation is critical! Many species are endangered due to habitat loss and poaching. We must protect their nesting sites and educate others about their importance.");
                                
                                conservationModule.AddOption("How can I help?",
                                    ply => true,
                                    ply => { pl.SendGump(new DialogueGump(pl, new DialogueModule("You can support conservation organizations, volunteer for beach clean-ups, and spread awareness about turtles! Every small effort counts."))); });
                                
                                conservationModule.AddOption("What organizations do you recommend?",
                                    plu => true,
                                    plu => 
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("I recommend the Sea Turtle Conservancy and World Wildlife Fund. They do excellent work for turtle conservation.")));
                                    });
                                
                                turtleLoveModule.AddOption("That sounds important.",
                                    pli => true,
                                    pli => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed, it is! Every creature plays a part in the balance of our world."))); });
                            });
                        
                        player.SendGump(new DialogueGump(player, speciesModule));
                    });

                turtleLoveModule.AddOption("Do you have a turtle companion?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule companionModule = new DialogueModule("Ah, I wish! I’ve often thought of adopting a turtle as a companion. They would be a noble and wise presence by my side.");
                        
                        companionModule.AddOption("What would you name your turtle?",
                            plo => true,
                            plo => { pl.SendGump(new DialogueGump(pl, new DialogueModule("I would name him 'Sir Shellington'! A name befitting a knight's companion."))); });
                        
                        companionModule.AddOption("Would you train it?",
                            plp => true,
                            plp => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Training a turtle would be quite the challenge, but it would be rewarding! They can learn to recognize their name and come when called."))); });
                        
                        player.SendGump(new DialogueGump(player, companionModule));
                    });

                player.SendGump(new DialogueGump(player, turtleLoveModule));
            });

        greeting.AddOption("What virtues do you follow?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("Ah, virtues guide my every action as a knight! Which virtue would you like to discuss?");
                
                virtuesModule.AddOption("Honesty",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Honesty is the foundation of trust. As a knight, I must uphold the truth."))); });

                virtuesModule.AddOption("Compassion",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Compassion drives me to help those in need, much like how we must care for our environment and the turtles within it."))); });

                virtuesModule.AddOption("Valor",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Valor is not just bravery; it's standing up for what is right, like advocating for the protection of turtles."))); });

                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player => { player.SendMessage("Safe travels, noble friend. May your path be as steady as a turtle's."); });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
