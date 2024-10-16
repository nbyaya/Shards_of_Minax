using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Vladimir Pochor")]
    public class VladimirPochor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public VladimirPochor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Vladimir Pochor";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 70;

            // Appearance
            AddItem(new ChainChest() { Hue = 1157 });
            AddItem(new ChainLegs() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new Boots() { Hue = 1157 });
            AddItem(new BoneHelm() { Hue = 1157 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Greetings, I am Vladimir Pochor, a guardian of ancient Slavic lore. Do you seek to uncover the legends?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To uncover the legends, you must first understand their elements. Ask me about my role or the lore.");
            }
            else if (speech.Contains("role"))
            {
                Say("My role is to protect and share the tales of Slavic mythology. The lore is vast and filled with ancient secrets.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of Slavic mythology are hidden in the tales of gods and heroes. Seek more about these tales or the deities.");
            }
            else if (speech.Contains("tales"))
            {
                Say("The tales of Slavic mythology speak of gods, spirits, and mystical creatures. Each tale holds a fragment of the truth.");
            }
            else if (speech.Contains("gods"))
            {
                Say("The gods of Slavic mythology include Perun, Veles, and Mokosh. Each has their own story and significance.");
            }
            else if (speech.Contains("mokosh"))
            {
                Say("Mokosh is the goddess of fertility and women's work. She is revered in many tales and ceremonies.");
            }
            else if (speech.Contains("fertility"))
            {
                Say("Indeed, fertility is a key aspect of Slavic mythology. Mokosh is central to this concept.");
            }
            else if (speech.Contains("perun"))
            {
                Say("Perun is the god of thunder and lightning, often depicted wielding a mighty axe. He is a fierce protector of the people.");
            }
            else if (speech.Contains("thunder"))
            {
                Say("Thunder is one of Perun's symbols, representing his power and might. The sound of thunder signifies his presence.");
            }
            else if (speech.Contains("lightning"))
            {
                Say("Lightning, wielded by Perun, is both a weapon and a symbol of divine power. It strikes with immense force and clarity.");
            }
            else if (speech.Contains("axe"))
            {
                Say("Perun's axe is a mighty weapon, a symbol of his authority and strength. It is said to strike fear into the hearts of his enemies.");
            }
            else if (speech.Contains("heroes"))
            {
                Say("The Slavic heroes often battled against dark forces and mythical beasts. Their stories are as rich as the gods' tales.");
            }
            else if (speech.Contains("beasts"))
            {
                Say("Mythical beasts in Slavic lore include the Zmey Gorynych, a fearsome dragon, and the Rusalka, a water spirit.");
            }
            else if (speech.Contains("zmay gorynych"))
            {
                Say("The Zmey Gorynych is a three-headed dragon that breathes fire and is a powerful antagonist in many tales.");
            }
            else if (speech.Contains("rusalka"))
            {
                Say("The Rusalka is a water spirit, often associated with the souls of young women. She can be both a benevolent and malevolent force.");
            }
            else if (speech.Contains("benevolent"))
            {
                Say("When the Rusalka is benevolent, she may offer guidance or aid to those lost. But be cautious, for her moods can shift.");
            }
            else if (speech.Contains("aid"))
            {
                Say("Aid from the Rusalka is rare and often comes with a price. The spirit of the waters is not easily swayed.");
            }
            else if (speech.Contains("price"))
            {
                Say("The price for aid from mythical beings can be great. It often involves a quest or a sacrifice.");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest is a journey or challenge that tests one's worth. Completing it often reveals great rewards or truths.");
            }
            else if (speech.Contains("truths"))
            {
                Say("The truths revealed through quests are often profound, revealing deeper knowledge or magical items.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge of the ancient lore can lead to great power and understanding. It is worth seeking with diligence.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power gained through knowledge must be wielded wisely. The ancient lore holds both wisdom and peril.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the ability to apply knowledge effectively. It is crucial for navigating the challenges of Slavic myths.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("The challenges faced in these myths often involve tests of strength, courage, and cunning.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is both physical and mental. Many heroes of the tales possessed great strength and fortitude.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the heart of a hero. It is the bravery to face the unknown and confront fears head-on.");
            }
            else if (speech.Contains("hero"))
            {
                Say("The heroes of Slavic mythology are celebrated for their bravery and deeds. They are central figures in many legends.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Your journey through the lore has been impressive. If you have answered the questions correctly, you may be worthy of a reward.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy, you must have proven your knowledge of the Slavic legends. If you have, accept this Slavic Legends Chest as a token of your achievement.");
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new SlavicLegendsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("You have already received a reward recently. Please return later.");
                }
            }
            else
            {
                Say("I do not understand that question. Perhaps you could ask about something related to the Slavic legends?");
            }

            base.OnSpeech(e);
        }

        public VladimirPochor(Serial serial) : base(serial) { }

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
