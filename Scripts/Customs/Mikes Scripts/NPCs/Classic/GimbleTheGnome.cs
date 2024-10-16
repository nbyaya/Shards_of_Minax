using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

public class GimbleTheGnome : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public GimbleTheGnome() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Gimble the Gnome";
        Body = 0x1C; // Gnome body
        Hue = 0xB;   // Skin color for gnomes

        // Stats
        SetStr(80);
        SetDex(80);
        SetInt(80);
        SetHits(60);

        // Appearance
        AddItem(new Robe() { Hue = 1153 });
        AddItem(new FloppyHat() { Hue = 1153 });
        AddItem(new Sandals() { Hue = 1153 });
        AddItem(new GnarledStaff() { Name = "Gimble's Lute" });

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public GimbleTheGnome(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Gimble the Gnome! Have you ever visited our marvelous city, where magic and technology blend seamlessly?");
        
        greeting.AddOption("Tell me about your city.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateCityModule()));
            });

        greeting.AddOption("How are you?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateHealthModule()));
            });

        greeting.AddOption("What do you do?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I'm a gnome of many talents, but my main job is tending to the ancient garden nearby.");
                jobModule.AddOption("Tell me more about the garden.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGardenModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        return greeting;
    }

    private DialogueModule CreateCityModule()
    {
        DialogueModule cityModule = new DialogueModule("Ah, our city is a marvel to behold! It's a place where ingenuity meets magic, filled with enchanting contraptions and spellbinding inventions. Would you like to know about our famous inventions or the magical creatures that inhabit our city?");
        
        cityModule.AddOption("Tell me about your inventions.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateInventionsModule()));
            });

        cityModule.AddOption("What magical creatures live there?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateCreaturesModule()));
            });

        return cityModule;
    }

    private DialogueModule CreateInventionsModule()
    {
        DialogueModule inventionsModule = new DialogueModule("Our gnomes are famed for their ingenious devices! For instance, we have the Gnomish Clockwork Dragon, a magnificent creation that soars through the skies. Would you like to hear about our other inventions?");
        
        inventionsModule.AddOption("Yes, please!",
            player => true,
            player =>
            {
                DialogueModule detailsModule = new DialogueModule("We also have the Sprocket-Powered Carriage, which zips around town at remarkable speeds! Additionally, our Arcane Energizers harness magical energy to power various machines. What specific invention are you curious about?");
                
                detailsModule.AddOption("Tell me more about the Clockwork Dragon.",
                    playerq => true,
                    playerq =>
                    {
                        DialogueModule dragonModule = new DialogueModule("The Clockwork Dragon is a masterpiece! It's powered by enchanted gears and can breathe magical fire. Every year, we host a grand festival where it performs aerial displays. Have you ever seen a dragon fly?");
                        dragonModule.AddOption("No, but I'd love to!",
                            pl => true,
                            pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                        dragonModule.AddOption("That sounds incredible!",
                            pl => true,
                            pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                        player.SendGump(new DialogueGump(player, dragonModule));
                    });

                detailsModule.AddOption("What about the Sprocket-Powered Carriage?",
                    playerw => true,
                    playerw =>
                    {
                        DialogueModule carriageModule = new DialogueModule("The Sprocket-Powered Carriage is a wonder in its own right! It runs on a unique blend of gears and spells, allowing it to traverse any terrain swiftly. It's quite a sight to see! Would you want to ride in one?");
                        carriageModule.AddOption("Absolutely!",
                            pl => true,
                            pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                        carriageModule.AddOption("I prefer my own two feet.",
                            pl => true,
                            pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                        player.SendGump(new DialogueGump(player, carriageModule));
                    });

                player.SendGump(new DialogueGump(player, detailsModule));
            });

        inventionsModule.AddOption("Maybe another time.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return inventionsModule;
    }

    private DialogueModule CreateCreaturesModule()
    {
        DialogueModule creaturesModule = new DialogueModule("Our city is home to many magical creatures! You might encounter the friendly Sprites, who help with our gardens, or the elusive Gnomefox, known for its cunning and intelligence. Would you like to hear about a specific creature?");
        
        creaturesModule.AddOption("Tell me about the Sprites.",
            player => true,
            player =>
            {
                DialogueModule spriteModule = new DialogueModule("Sprites are delightful little beings! They flit about, spreading joy and tending to our flowers. They have a special bond with nature. Have you ever seen one in action?");
                spriteModule.AddOption("No, I haven't.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                spriteModule.AddOption("That sounds magical!",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                player.SendGump(new DialogueGump(player, spriteModule));
            });

        creaturesModule.AddOption("What about the Gnomefox?",
            player => true,
            player =>
            {
                DialogueModule gnomefoxModule = new DialogueModule("The Gnomefox is a curious creature! It’s known for its bright orange fur and playful nature. They often steal trinkets from our workshops but are too cute to be scolded! Have you ever encountered a mischievous creature?");
                gnomefoxModule.AddOption("I can't say that I have.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                gnomefoxModule.AddOption("That sounds fun!",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                player.SendGump(new DialogueGump(player, gnomefoxModule));
            });

        creaturesModule.AddOption("Tell me more about the magical beasts.",
            player => true,
            player =>
            {
                DialogueModule beastsModule = new DialogueModule("We also have majestic creatures like the Skywhale, which glides gracefully through the clouds, and the Forest Guardian, a wise and ancient being that protects our lands. Would you like to learn about their stories?");
                
                beastsModule.AddOption("Yes, I'd love to hear their stories!",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateBeastsStoriesModule())); });
                beastsModule.AddOption("Maybe later.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                player.SendGump(new DialogueGump(player, beastsModule));
            });

        return creaturesModule;
    }

    private DialogueModule CreateBeastsStoriesModule()
    {
        DialogueModule storiesModule = new DialogueModule("The Skywhale is a gentle giant, often seen soaring above the city, casting shadows on the streets. Legend says it can control the weather. The Forest Guardian is a majestic entity, appearing only to those who show true respect for nature. Have you heard of such creatures before?");
        
        storiesModule.AddOption("I have not, but they sound fascinating!",
            pl => true,
            pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
        storiesModule.AddOption("I love stories about magical creatures!",
            pl => true,
            pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });

        return storiesModule;
    }

    private DialogueModule CreateHealthModule()
    {
        return new DialogueModule("I'm in fine fettle, I am!");
    }

    private DialogueModule CreateGardenModule()
    {
        DialogueModule gardenModule = new DialogueModule("The garden is a place of beauty and serenity, a testament to the virtue of Humility. Would you like to learn more about it?");
        
        gardenModule.AddOption("Yes, I want to learn more about gardening.",
            player => true,
            player =>
            {
                DialogueModule gardeningModule = new DialogueModule("Gardening is not just about planting seeds and watching them grow. It's about patience, understanding, and nurturing. If you're ever interested, I can offer you some seeds to start your own garden. Interested?");
                gardeningModule.AddOption("Yes, I would love some seeds!",
                    pl => CanReceiveSeeds(pl),
                    pl =>
                    {
                        GiveSeeds(pl);
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                gardeningModule.AddOption("Maybe later.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, gardeningModule));
            });
        return gardenModule;
    }

    private bool CanReceiveSeeds(PlayerMobile player)
    {
        return DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10);
    }

    private void GiveSeeds(PlayerMobile player)
    {
        Say("Wonderful! Here, take these seeds. They are a special variety that I've cultivated over the years. May they bring joy and wisdom to your life. Remember, patience is key.");
        player.AddToBackpack(new ShirtSlotChangeDeed()); // Give the reward
        lastRewardTime = DateTime.UtcNow; // Update the timestamp
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
