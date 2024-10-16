using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Master Zalazar")]
    public class MasterZalazar : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MasterZalazar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Master Zalazar";
            Body = 0x190; // Human male body

            // Stats
            SetStr(80);
            SetDex(40);
            SetInt(100);
            SetHits(60);

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new WizardsHat() { Hue = 1153 });
            AddItem(new MortarPestle() { Name = "Zalazar's Mortar and Pestle" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public MasterZalazar(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Master Zalazar, a seeker of wisdom. How may I assist you today?");

            greeting.AddOption("Tell me about yourself.",
                player => true,
                player =>
                {
                    DialogueModule aboutModule = new DialogueModule("I am Master Zalazar, a seeker of wisdom and an ardent follower of the virtues. My life is devoted to understanding the mysteries of the world.");
                    aboutModule.AddOption("What mysteries do you seek?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateMysteryModule())));
                    player.SendGump(new DialogueGump(player, aboutModule));
                });

            greeting.AddOption("What do you think of health?",
                player => true,
                player =>
                {
                    DialogueModule healthModule = new DialogueModule("I am in perfect health, both in body and mind. True health comes from the balance of mind, body, and spirit.");
                    healthModule.AddOption("How can I achieve balance?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateBalanceModule())));
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My purpose is to seek knowledge and understanding. I study the ancient texts and pursue the path of the virtues.");
                    jobModule.AddOption("What are the virtues?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateVirtueModule())));
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("Tell me about meditation.",
                player => true,
                player =>
                {
                    DialogueModule meditateModule = new DialogueModule("Meditation is a practice that calms the soul and sharpens the mind. It allows one to connect with the deeper truths of existence.");
                    meditateModule.AddOption("What do you visualize during meditation?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateMeditationModule())));
                    player.SendGump(new DialogueGump(player, meditateModule));
                });

            greeting.AddOption("Do you have a reward for me?",
                player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                player =>
                {
                    player.SendMessage("I have a small token for those who show true honor. Would you like it?");
                    player.AddToBackpack(new BlacksmithyAugmentCrystal()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                });

            greeting.AddOption("What about the ruins?",
                player => true,
                player =>
                {
                    DialogueModule ruinsModule = new DialogueModule("The ruins to the East are remnants of an ancient civilization. They hold secrets and treasures. But beware, for they are also guarded by creatures of old.");
                    ruinsModule.AddOption("What kind of creatures?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateCreatureModule())));
                    player.SendGump(new DialogueGump(player, ruinsModule));
                });

            greeting.AddOption("What do you know of wisdom?",
                player => true,
                player =>
                {
                    player.SendMessage("Wisdom is the light that pierces the darkness of ignorance. It requires experience, reflection, and a willingness to learn.");
                });

            return greeting;
        }

        private DialogueModule CreateMysteryModule()
        {
            DialogueModule mysteryModule = new DialogueModule("I seek the wisdom of the ancients, hidden within the mysterious ruins of old. Each text holds a piece of the puzzle.");
            mysteryModule.AddOption("What have you found?",
                p => true,
                p =>
                {
                    p.SendMessage("I have uncovered many insights, but the true knowledge lies just beyond my grasp. I hope to find it with the help of brave adventurers.");
                });
            mysteryModule.AddOption("Can I help you?",
                p => true,
                p => p.SendMessage("Your offer is noble! If you journey to the East, perhaps you can bring back more ancient texts or artifacts."));
            return mysteryModule;
        }

        private DialogueModule CreateBalanceModule()
        {
            DialogueModule balanceModule = new DialogueModule("Achieving balance requires discipline and self-awareness. Consider daily practices that nurture your mind, body, and spirit.");
            balanceModule.AddOption("What practices do you suggest?",
                p => true,
                p => p.SendMessage("I recommend meditation, physical exercise, and engaging in thoughtful discussions with others."));
            return balanceModule;
        }

        private DialogueModule CreateVirtueModule()
        {
            DialogueModule virtueModule = new DialogueModule("The virtues are Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each one is essential to a noble life.");
            virtueModule.AddOption("Tell me about Honesty.",
                p => true,
                p => p.SendMessage("Honesty is the foundation of trust. Without it, relationships crumble and society falters."));
            virtueModule.AddOption("Tell me about Compassion.",
                p => true,
                p => p.SendMessage("Compassion allows us to understand and empathize with others. It is the bedrock of a caring community."));
            virtueModule.AddOption("Tell me about Valor.",
                p => true,
                p => p.SendMessage("Valor is the courage to face fear and adversity, standing firm in one’s beliefs and actions."));
            virtueModule.AddOption("Tell me about Justice.",
                p => true,
                p => p.SendMessage("Justice ensures fairness and equality, a vital aspect of a functioning society."));
            virtueModule.AddOption("Tell me about Sacrifice.",
                p => true,
                p => p.SendMessage("Sacrifice is giving without expecting anything in return. It is a selfless act for the benefit of others."));
            virtueModule.AddOption("Tell me about Honor.",
                p => true,
                p => p.SendMessage("Honor means living with integrity and standing by one’s principles, even when challenged."));
            virtueModule.AddOption("Tell me about Spirituality.",
                p => true,
                p => p.SendMessage("Spirituality is the pursuit of a deeper connection with the universe, understanding one’s place within it."));
            virtueModule.AddOption("Tell me about Humility.",
                p => true,
                p => p.SendMessage("Humility allows us to acknowledge our limitations and remain open to learning from others."));
            return virtueModule;
        }

        private DialogueModule CreateMeditationModule()
        {
            DialogueModule meditateModule = new DialogueModule("During meditation, I visualize a serene mountain peak, a place of clarity and peace. It helps me center my thoughts.");
            meditateModule.AddOption("How can I meditate effectively?",
                p => true,
                p => p.SendMessage("Find a quiet place, sit comfortably, and focus on your breath. Allow your thoughts to come and go without attachment."));
            meditateModule.AddOption("What have you seen in your meditations?",
                p => true,
                p => p.SendMessage("I have glimpsed ancient landscapes and heard whispers of forgotten knowledge. It inspires my quest for understanding."));
            return meditateModule;
        }

        private DialogueModule CreateCreatureModule()
        {
            DialogueModule creatureModule = new DialogueModule("The creatures guarding the ruins are as varied as they are dangerous. You may encounter spectral guardians, ancient constructs, or wild beasts.");
            creatureModule.AddOption("What should I prepare for?",
                p => true,
                p => p.SendMessage("Be ready for combat, but also be wise. Sometimes a clever approach can avoid confrontation altogether."));
            creatureModule.AddOption("Have you ventured there yourself?",
                p => true,
                p => p.SendMessage("I have visited the outskirts but have yet to delve into the heart of the ruins. Fear holds me back, though I yearn to explore."));
            return creatureModule;
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
}
