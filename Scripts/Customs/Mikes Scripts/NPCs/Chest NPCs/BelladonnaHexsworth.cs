using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Belladonna Hexsworth")]
    public class BelladonnaHexsworth : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BelladonnaHexsworth() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Belladonna Hexsworth";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = 1175 }); // Witchy robe hue
            AddItem(new WizardsHat() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1175 });
            AddItem(new Spellbook() { Name = "Belladonna's Grimoire" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x203D); // Various hair styles
            HairHue = Utility.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Belladonna Hexsworth, keeper of magical brews and arcane secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as vibrant as a potion brewed under the full moon.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to concoct potions and maintain the balance of mystical energies.");
            }
            else if (speech.Contains("potions"))
            {
                Say("Potions are my specialty. Some can heal, others may bring misfortune. It all depends on the ingredients and intent.");
            }
            else if (speech.Contains("ingredients"))
            {
                Say("Ah, ingredients. The essence of alchemy. They come from rare herbs, mystical creatures, and sometimes, the very essence of magic itself.");
            }
            else if (speech.Contains("alchemy"))
            {
                Say("Alchemy is the art of transformation. It blends ingredients to create magical effects, from healing to harm.");
            }
            else if (speech.Contains("transformation"))
            {
                Say("Transformation is a powerful magic. It can alter the very essence of objects or beings. Always handle with care.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is the force that shapes our world. It is present in every potion, every spell, and every mystical artifact.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("Artifacts are imbued with potent magic. They can hold incredible power, and some even have minds of their own.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power can be both a gift and a curse. It must be wielded wisely, lest it corrupt those who seek it.");
            }
            else if (speech.Contains("corruption"))
            {
                Say("Corruption often stems from misuse of power. It taints the soul and twists intentions. Purity of purpose is essential.");
            }
            else if (speech.Contains("purity"))
            {
                Say("Purity is the essence of true magic. It is the clarity of intention and the cleanliness of one's heart.");
            }
            else if (speech.Contains("intention"))
            {
                Say("Intention guides the magic. Whether you seek to heal or harm, your intention will shape the outcome of the spell.");
            }
            else if (speech.Contains("heal"))
            {
                Say("Healing is a noble pursuit. It mends wounds and restores vitality, bringing balance and restoration.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Maintaining balance is crucial in magic. Too much of one element can disrupt the harmony of the brew and bring unintended consequences.");
            }
            else if (speech.Contains("brew"))
            {
                Say("Brewing is an art. It requires patience and precision. A single misstep can turn a healing elixir into a dreadful curse.");
            }
            else if (speech.Contains("curse"))
            {
                Say("A curse is a dark magic that brings misfortune. It can be countered with equal measures of resolve and protection.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the strength of character. It is what allows one to face challenges and overcome adversity.");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversity tests our resolve and character. It is through overcoming trials that we grow and learn.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth is the outcome of learning and experience. It is both personal and mystical, shaping one's path.");
            }
            else if (speech.Contains("path"))
            {
                Say("The path of magic is winding and unpredictable. It requires wisdom and caution to navigate successfully.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is gained through experience and reflection. It helps one to make informed decisions and avoid pitfalls.");
            }
            else if (speech.Contains("decision"))
            {
                Say("Every decision shapes the future. Consider your choices carefully, for they will determine your path.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future is shaped by the present. Every action and choice has consequences that ripple through time.");
            }
            else if (speech.Contains("consequence"))
            {
                Say("Consequences are the results of our actions. They can be positive or negative, but they always provide lessons.");
            }
            else if (speech.Contains("lesson"))
            {
                Say("Lessons are the teachings we learn from experience. They guide us towards growth and understanding.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding brings clarity. It allows one to see beyond the surface and grasp the deeper meanings of magic and life.");
            }
            else if (speech.Contains("deeper"))
            {
                Say("The deeper aspects of magic are hidden from the untrained eye. They require dedication and study to uncover.");
            }
            else if (speech.Contains("study"))
            {
                Say("Study is the key to mastering magic. It involves learning from texts, practicing spells, and understanding magical principles.");
            }
            else if (speech.Contains("principle"))
            {
                Say("Principles are the fundamental truths that underpin magic. They are the rules that govern the magical world.");
            }
            else if (speech.Contains("rules"))
            {
                Say("Rules in magic are like laws of nature. They guide how magic operates and ensure stability and balance.");
            }
            else if (speech.Contains("stability"))
            {
                Say("Stability in magic ensures that spells and potions have predictable effects. It is crucial for reliable results.");
            }
            else if (speech.Contains("results"))
            {
                Say("Results are the outcomes of magical endeavors. They can vary widely, depending on the skill and intention behind the magic.");
            }
            else if (speech.Contains("skill"))
            {
                Say("Skill in magic comes with practice and experience. It is honed through continual learning and application.");
            }
            else if (speech.Contains("application"))
            {
                Say("Application of magical knowledge is where theory meets practice. It is the testing ground for one's skill.");
            }
            else if (speech.Contains("testing"))
            {
                Say("Testing magical theories and spells helps to refine and perfect them. It is an essential part of the learning process.");
            }
            else if (speech.Contains("perfection"))
            {
                Say("Perfection in magic is a journey, not a destination. It involves constant refinement and improvement.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey of magic is filled with discovery and wonder. It is a path of exploration and growth.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery reveals new aspects of magic and the world. It is through exploration that we expand our understanding.");
            }
            else if (speech.Contains("exploration"))
            {
                Say("Exploration of magical realms and knowledge broadens one's horizons and enriches the magical experience.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience is the accumulation of knowledge and skill over time. It shapes one's mastery of magic.");
            }
            else if (speech.Contains("mastery"))
            {
                Say("Mastery in magic represents a high level of skill and understanding. It is achieved through dedication and practice.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication to the study and practice of magic leads to mastery. It requires perseverance and passion.");
            }
            else if (speech.Contains("passion"))
            {
                Say("Passion drives one's pursuit of magical knowledge. It fuels the quest for understanding and excellence.");
            }
            else if (speech.Contains("quest"))
            {
                Say("The quest for magical knowledge is a lifelong journey. It is filled with challenges, rewards, and personal growth.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges test our abilities and resolve. Overcoming them leads to growth and deeper understanding.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth is the ultimate goal of any magical journey. It signifies progress and enlightenment.");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Enlightenment is the realization of profound truths. It is the culmination of one's magical quest and learning.");
            }
            else if (speech.Contains("truths"))
            {
                Say("Truths in magic are the foundational principles that guide its practice. Understanding them brings deeper insight.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight into magic provides clarity and understanding. It allows one to see beyond illusions and grasp the essence of magic.");
            }
            else if (speech.Contains("illusion"))
            {
                Say("Illusions can deceive and mislead. It is important to see through them to uncover the true nature of magic.");
            }
            else if (speech.Contains("nature"))
            {
                Say("The nature of magic is both wondrous and mysterious. It encompasses the essence of transformation and power.");
            }
            else if (speech.Contains("transformation"))
            {
                Say("Transformation is the essence of magic. It allows one to alter reality and change the very fabric of existence.");
            }
            else if (speech.Contains("existence"))
            {
                Say("Existence is the state of being. Magic can shape and influence it in profound ways.");
            }
            else if (speech.Contains("influence"))
            {
                Say("Influence in magic is the power to effect change. It can be wielded for good or ill, depending on the wielder's intentions.");
            }
            else if (speech.Contains("wielder"))
            {
                Say("The wielder of magic must be wise and responsible. Their intentions and actions will shape the outcomes of their spells.");
            }
            else if (speech.Contains("responsible"))
            {
                Say("Responsibility in magic involves using one's power wisely and ethically. It ensures that magic is used for the greater good.");
            }
            else if (speech.Contains("good"))
            {
                Say("Good in magic is about using one's powers to benefit others and uphold positive values.");
            }
            else if (speech.Contains("values"))
            {
                Say("Values guide one's actions and decisions. In magic, they ensure that spells and potions are used with integrity.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the adherence to moral and ethical principles. It is crucial for maintaining trust and respect in magic.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is the foundation of all relationships, including those in magic. It ensures that one's intentions are respected and valued.");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect for magic and its practitioners fosters a positive and harmonious environment. It is essential for collaboration and learning.");
            }
            else if (speech.Contains("collaboration"))
            {
                Say("Collaboration in magic can lead to greater discoveries and advancements. Working together brings diverse perspectives and strengths.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery in magic is the process of uncovering new knowledge and understanding. It enriches the magical practice.");
            }
            else if (speech.Contains("practice"))
            {
                Say("Practice refines one's magical abilities. It is through consistent effort that one achieves mastery and insight.");
            }
            else if (speech.Contains("mastery"))
            {
                Say("Mastery represents the pinnacle of magical skill. It is achieved through dedication, practice, and a deep understanding of magical principles.");
            }
            else if (speech.Contains("achievement"))
            {
                Say("Achievement in magic signifies the successful application of knowledge and skills. It is the result of perseverance and dedication.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is the key to overcoming obstacles and achieving magical goals. It requires persistence and determination.");
            }
            else if (speech.Contains("determination"))
            {
                Say("Determination fuels the pursuit of magical excellence. It drives one to overcome challenges and achieve greatness.");
            }
            else if (speech.Contains("greatness"))
            {
                Say("Greatness in magic is the culmination of skill, knowledge, and integrity. It represents the highest level of achievement.");
            }
            else if (speech.Contains("achievement"))
            {
                Say("Achievement in magic signifies the successful application of knowledge and skills. It is the result of perseverance and dedication.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication to the pursuit of magical knowledge leads to greater understanding and mastery. It is essential for success.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success in magic is the result of hard work, dedication, and skill. It brings rewards and fulfillment.");
            }
            else if (speech.Contains("fulfillment"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You have proven yourself worthy of my trust. For your curiosity and spirit, accept this Witch's Brew Chest, filled with magical treasures.");
                    from.AddToBackpack(new WitchsBrewChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BelladonnaHexsworth(Serial serial) : base(serial) { }

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
