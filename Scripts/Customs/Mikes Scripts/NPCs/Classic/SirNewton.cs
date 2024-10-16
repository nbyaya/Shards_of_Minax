using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Sir Newton")]
public class SirNewton : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SirNewton() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sir Newton";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(75);
        SetInt(100);
        SetHits(70);

        // Appearance
        AddItem(new LongPants() { Hue = 1122 });
        AddItem(new Tunic() { Hue = 1122 });
        AddItem(new Boots() { Hue = 1122 });
        AddItem(new Spellbook() { Name = "Principia Mathematica" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("I am Sir Newton, the great philosopher. What do you want, peasant? I must say, the alchemy here is quite remarkable!");

        greeting.AddOption("Tell me more about alchemy.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateAlchemyModule()));
            });

        greeting.AddOption("What do you think of this realm?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateRealmOpinionModule()));
            });

        greeting.AddOption("Do you have any philosophical insights?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("I must go.",
            player => true,
            player => {
                Say("Before you leave, remember this: Ignorance can be cured with knowledge. Seek out the library of the ancients to better yourself.");
            });

        return greeting;
    }

    private DialogueModule CreateAlchemyModule()
    {
        DialogueModule alchemyModule = new DialogueModule("Ah, alchemy! It is a wondrous art here! The techniques, the potions—such creativity! What would you like to know about it?");
        
        alchemyModule.AddOption("What are the key ingredients for alchemy?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateIngredientsModule()));
            });

        alchemyModule.AddOption("Can you teach me about potion-making?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreatePotionMakingModule()));
            });

        alchemyModule.AddOption("What do you think of alchemical creatures?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateCreatureModule()));
            });

        alchemyModule.AddOption("I have other questions.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateGeneralQuestionsModule()));
            });

        return alchemyModule;
    }

    private DialogueModule CreateIngredientsModule()
    {
        DialogueModule ingredientsModule = new DialogueModule("The key ingredients include rare herbs, mystical essences, and elemental crystals. Each has unique properties! Would you like to hear about any specific one?");
        
        ingredientsModule.AddOption("Tell me about herbs.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateHerbsModule()));
            });

        ingredientsModule.AddOption("What about essences?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateEssencesModule()));
            });

        ingredientsModule.AddOption("What are elemental crystals?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateCrystalsModule()));
            });

        ingredientsModule.AddOption("I want to know more about something else.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateGeneralQuestionsModule()));
            });

        return ingredientsModule;
    }

    private DialogueModule CreateHerbsModule()
    {
        return new DialogueModule("Herbs are the backbone of many potions! For example, Moonlit Petals are known for their healing properties, while Nightshade can enhance stealth. It’s fascinating how nature aids our craft!");
    }

    private DialogueModule CreateEssencesModule()
    {
        return new DialogueModule("Essences, such as Essence of Life, are extracted from mystical creatures. They can enhance your potions significantly! Have you ever seen one?");
    }

    private DialogueModule CreateCrystalsModule()
    {
        return new DialogueModule("Elemental crystals are rare! Fire, Water, Earth, and Air crystals all hold unique powers. Using them wisely can lead to astonishing results in your experiments!");
    }

    private DialogueModule CreatePotionMakingModule()
    {
        DialogueModule potionMakingModule = new DialogueModule("Potion-making is an art form! You must balance ingredients carefully. What type of potion interests you?");
        
        potionMakingModule.AddOption("Healing potions.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateHealingPotionModule()));
            });

        potionMakingModule.AddOption("Buff potions.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateBuffPotionModule()));
            });

        potionMakingModule.AddOption("Explosive potions.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateExplosivePotionModule()));
            });

        potionMakingModule.AddOption("I want to learn about something else.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateGeneralQuestionsModule()));
            });

        return potionMakingModule;
    }

    private DialogueModule CreateHealingPotionModule()
    {
        return new DialogueModule("Healing potions are crafted using Moonlit Petals and Crystal Dew. The trick is in the timing—mix them at dawn for the best results!");
    }

    private DialogueModule CreateBuffPotionModule()
    {
        return new DialogueModule("Buff potions often include Dragon’s Breath and various elemental essences. They enhance your abilities for a short duration, perfect for battles!");
    }

    private DialogueModule CreateExplosivePotionModule()
    {
        return new DialogueModule("Explosive potions require rare components, like Fire crystals and volatile liquids. Use them wisely, as they can be quite dangerous!");
    }

    private DialogueModule CreateCreatureModule()
    {
        DialogueModule creatureModule = new DialogueModule("Alchemical creatures are fascinating! They often possess unique traits and abilities. Would you like to know about a specific type?");
        
        creatureModule.AddOption("Tell me about Elementals.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateElementalsModule()));
            });

        creatureModule.AddOption("What about Homunculi?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateHomunculiModule()));
            });

        creatureModule.AddOption("I want to know about another creature.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateGeneralQuestionsModule()));
            });

        return creatureModule;
    }

    private DialogueModule CreateElementalsModule()
    {
        return new DialogueModule("Elementals are manifestations of elemental forces. They can be summoned and controlled by skilled alchemists. Have you ever encountered one?");
    }

    private DialogueModule CreateHomunculiModule()
    {
        return new DialogueModule("Homunculi are created through alchemical processes. They can serve various purposes, from labor to companionship. Crafting one is a true art!");
    }

    private DialogueModule CreateRealmOpinionModule()
    {
        DialogueModule realmOpinionModule = new DialogueModule("This realm! The alchemy here is far more advanced than where I come from. The creativity and knowledge are awe-inspiring!");

        realmOpinionModule.AddOption("What makes it so advanced?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateAdvancementModule()));
            });

        realmOpinionModule.AddOption("Do you feel at home here?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateHomeFeelModule()));
            });

        realmOpinionModule.AddOption("Tell me more about your journey.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateJourneyModule()));
            });

        return realmOpinionModule;
    }

    private DialogueModule CreateAdvancementModule()
    {
        return new DialogueModule("The techniques here are unparalleled! From potion-making to the manipulation of mystical energies, the knowledge is shared freely among practitioners.");
    }

    private DialogueModule CreateHomeFeelModule()
    {
        return new DialogueModule("Absolutely! The community is welcoming, and the pursuit of knowledge is celebrated. It feels good to be surrounded by such bright minds!");
    }

    private DialogueModule CreateJourneyModule()
    {
        return new DialogueModule("My journey has been long, filled with challenges and discoveries. I've learned from many masters and hope to share my knowledge with those eager to learn.");
    }

    private DialogueModule CreateGeneralQuestionsModule()
    {
        DialogueModule generalQuestionsModule = new DialogueModule("What else would you like to know? My mind is open to your inquiries!");

        generalQuestionsModule.AddOption("Do you have any philosophical insights?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        generalQuestionsModule.AddOption("What are your thoughts on knowledge?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateKnowledgeModule()));
            });

        generalQuestionsModule.AddOption("Tell me about rewards for curiosity.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateCuriosityRewardModule(player)));
            });

        return generalQuestionsModule;
    }

    private DialogueModule CreateKnowledgeModule()
    {
        return new DialogueModule("Knowledge is a treasure, far more valuable than gold! It empowers you and opens doors to new realms of understanding.");
    }

    private DialogueModule CreateCuriosityRewardModule(PlayerMobile player)
    {
        DialogueModule curiosityRewardModule = new DialogueModule("Curiosity is rewarded here! If you find something intriguing, bring it to me, and you may receive a gift!");

        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            curiosityRewardModule = new DialogueModule("I have no reward right now. Please return later.");
        }
        else
        {
            curiosityRewardModule.AddOption("What reward do you offer?",
                p => true,
                p => {
                    p.SendGump(new DialogueGump(p, CreateRewardModule(p)));
                });
        }

        return curiosityRewardModule;
    }

    private DialogueModule CreateRewardModule(PlayerMobile player)
    {
        lastRewardTime = DateTime.UtcNow; // Update the timestamp
        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
        return new DialogueModule("You have proven your curiosity. Here is your reward!");
    }

    public SirNewton(Serial serial) : base(serial) { }

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
