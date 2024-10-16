using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ZaraTheStarborn : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ZaraTheStarborn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zara the Starborn";
        Body = 0x191; // Human female body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Stats
        SetStr(50);
        SetDex(40);
        SetInt(150);
        SetHits(60);

        AddItem(new FancyDress() { Hue = 2951 });
        AddItem(new Circlet() { Hue = 2952 });
        AddItem(new Spellbook() { Name = "Zara's Starbook" });

        lastRewardTime = DateTime.MinValue;
    }

    public ZaraTheStarborn(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Zara the Starborn, a traveler from the colonies of the former Planar Imperium. What knowledge do you seek?");

        greeting.AddOption("Tell me about the Planar Imperium.",
            player => true,
            player =>
            {
                DialogueModule imperiumModule = new DialogueModule("The Planar Imperium was a vast realm, transcending the boundaries of our known world. It was a place where the fabric of reality was woven with magic. Would you like to know about its colonies?");
                imperiumModule.AddOption("Yes, tell me about the colonies.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateColoniesModule()));
                    });
                player.SendGump(new DialogueGump(player, imperiumModule));
            });

        greeting.AddOption("What was life like in the colonies?",
            player => true,
            player =>
            {
                DialogueModule lifeModule = new DialogueModule("Life in the colonies was vibrant yet challenging. The blend of cultures and magic shaped our daily lives. Would you like to hear more about the people or the magic?");
                lifeModule.AddOption("Tell me about the people.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreatePeopleModule()));
                    });
                lifeModule.AddOption("Tell me about the magic.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateMagicModule()));
                    });
                player.SendGump(new DialogueGump(player, lifeModule));
            });

        greeting.AddOption("Do you have a task for me?",
            player => CanAcceptTask(player),
            player =>
            {
                DialogueModule taskModule = new DialogueModule("The task I have is to find the Echoing Crystal hidden in this realm and bring it to me. In return, I will bestow upon you a reward.");
                taskModule.AddOption("I will find the Echoing Crystal.",
                    pl => true,
                    pl =>
                    {
                        GiveReward(pl);
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, taskModule));
            });

        return greeting;
    }

    private DialogueModule CreateColoniesModule()
    {
        DialogueModule coloniesModule = new DialogueModule("The colonies were scattered across many planes, each with its own unique resources and challenges. Some were lush and teeming with life, while others were barren and desolate. Do you wish to hear about a specific colony?");
        coloniesModule.AddOption("Tell me about the Crystal Plains.",
            pl => true,
            pl =>
            {
                DialogueModule crystalPlainsModule = new DialogueModule("Ah, the Crystal Plains! It was a realm of stunning beauty, where the ground sparkled like gems under the sun. We harvested crystals that enhanced our magic and crafted wondrous artifacts.");
                crystalPlainsModule.AddOption("What kind of artifacts?",
                    p => true,
                    p =>
                    {
                        DialogueModule artifactsModule = new DialogueModule("Artifacts of immense power, like the Shard of Echoes, which could amplify spells or the Prism of Clarity that revealed hidden truths. Would you like to know more about their creation?");
                        artifactsModule.AddOption("Yes, how were they created?",
                            plq => true,
                            plq =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, artifactsModule));
                    });
                pl.SendGump(new DialogueGump(pl, crystalPlainsModule));
            });
        coloniesModule.AddOption("Tell me about the Shadow Marshes.",
            pl => true,
            pl =>
            {
                DialogueModule shadowMarshesModule = new DialogueModule("The Shadow Marshes were eerie and filled with whispers of the past. Many sought power in the dark waters, but few returned unchanged. Do you wish to hear a tale from the marshes?");
                shadowMarshesModule.AddOption("Yes, tell me a tale.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Zara leans in, her voice low, recounting the tale of the Lost Ones who wandered the marshes...");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, shadowMarshesModule));
            });
        return coloniesModule;
    }

    private DialogueModule CreatePeopleModule()
    {
        DialogueModule peopleModule = new DialogueModule("The people of the colonies were diverse. We were a tapestry of cultures, united by our pursuit of knowledge and magic. Would you like to hear about a particular group?");
        peopleModule.AddOption("Tell me about the Starborn Clan.",
            pl => true,
            pl =>
            {
                DialogueModule starbornModule = new DialogueModule("The Starborn Clan was known for their mastery of celestial magic. They believed that the stars were not just lights in the sky but sentient beings guiding us. Do you wish to hear about their rituals?");
                starbornModule.AddOption("Yes, what rituals did they perform?",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Zara describes the rituals under the night sky, where they would align their energies with the stars...");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, starbornModule));
            });
        peopleModule.AddOption("What about the Earthbound?",
            pl => true,
            pl =>
            {
                DialogueModule earthboundModule = new DialogueModule("The Earthbound were practical folk, deeply connected to the land. They often crafted tools and artifacts that harnessed the elements. Would you like to know about their crafts?");
                earthboundModule.AddOption("Yes, tell me about their crafts.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Zara explains how they worked with the earth and its minerals, creating powerful items...");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, earthboundModule));
            });
        return peopleModule;
    }

    private DialogueModule CreateMagicModule()
    {
        DialogueModule magicModule = new DialogueModule("Magic in the colonies was woven into every aspect of life. It was both a tool and a way of being. Would you like to know about spellcasting techniques or magical artifacts?");
        magicModule.AddOption("Tell me about spellcasting techniques.",
            pl => true,
            pl =>
            {
                DialogueModule castingModule = new DialogueModule("We used various techniques, from incantations to gestures. Each spell had a unique rhythm. Would you like to hear about a specific spell?");
                castingModule.AddOption("Yes, tell me about a powerful spell.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Zara speaks of the Celestial Convergence, a spell that aligned the energies of the stars to create a massive burst of power...");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, castingModule));
            });
        magicModule.AddOption("What about magical artifacts?",
            pl => true,
            pl =>
            {
                DialogueModule artifactsModule = new DialogueModule("Artifacts were central to our magic. Some could bend reality, while others could heal or enhance the mind. Would you like to know about a particular artifact?");
                artifactsModule.AddOption("Yes, tell me about the Shard of Echoes.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Zara describes how the Shard was forged from the tears of a dying star, amplifying the user's magical abilities...");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, artifactsModule));
            });
        return magicModule;
    }

    private bool CanAcceptTask(PlayerMobile player)
    {
        return DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10);
    }

    private void GiveReward(PlayerMobile player)
    {
        player.AddToBackpack(new StaminaLeechAugmentCrystal());
        lastRewardTime = DateTime.UtcNow;
        player.SendMessage("You have received a Stamina Leech Augment Crystal!");
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
