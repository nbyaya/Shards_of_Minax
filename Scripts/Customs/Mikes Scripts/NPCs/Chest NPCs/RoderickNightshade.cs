using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Roderick Nightshade")]
    public class RoderickNightshade : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool hasRevealedSecrets;
        private bool hasMentionedDarkness;
        private bool hasMentionedMondain;
        private bool hasMentionedReveal;
		private bool hasMentionedSecrets;

        [Constructable]
        public RoderickNightshade() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Roderick Nightshade";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new BlackStaff() { Hue = 1150 });
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new WizardsHat() { Hue = 1150 });
            AddItem(new FancyShirt() { Hue = 1150 });
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Shoes() { Hue = 1150 });
			
            Hue = Utility.RandomSkinHue(); // Random skin hue
            HairItemID = Utility.RandomList(0x204D, 0x203B); // Hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x204B, 0x204D); // Facial hair styles

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the dialogue state
            lastRewardTime = DateTime.MinValue;
            hasRevealedSecrets = false;
            hasMentionedDarkness = false;
            hasMentionedMondain = false;
            hasMentionedReveal = false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Roderick Nightshade, a keeper of ancient mysteries.");
                hasMentionedMondain = true; // Enable subsequent dialogues
            }
            else if (speech.Contains("health"))
            {
                Say("My health is shrouded in the veil of secrecy, just like the mysteries I guard.");
                hasMentionedDarkness = true; // Enable subsequent dialogues
            }
            else if (speech.Contains("job"))
            {
                Say("I protect and guard the ancient secrets that lie hidden in the darkness.");
                hasRevealedSecrets = true; // Enable subsequent dialogues
            }
            else if (hasMentionedMondain && speech.Contains("secrets"))
            {
                Say("Mondain's secrets are buried deep within the shadows. Only the persistent can uncover them.");
                hasMentionedReveal = true; // Enable subsequent dialogues
            }
            else if (hasMentionedDarkness && speech.Contains("darkness"))
            {
                Say("The darkness conceals many truths. Embrace it, and you may find what you seek.");
            }
            else if (hasMentionedReveal && speech.Contains("reveal"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The time for a reward has not yet arrived. Return when the moon is high.");
                }
                else
                {
                    Say("Your journey has led you to this moment. For your perseverance, take this chest and uncover its mysteries.");
                    from.AddToBackpack(new MondainsDarkSecretsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (hasRevealedSecrets && speech.Contains("mondain"))
            {
                Say("Mondain's name is whispered in the dark corners of the world. His legacy is intertwined with the secrets I guard.");
				hasMentionedSecrets = true; // Enable subsequent dialogues
            }
            else if (hasMentionedDarkness && speech.Contains("shadow"))
            {
                Say("Shadows are the extensions of darkness, and they hold many secrets. Seek them out if you dare.");
            }
            else if (hasMentionedSecrets && speech.Contains("puzzle"))
            {
                Say("This puzzle is part of the journey to uncover the truths that lie hidden. Each answer brings you closer.");
            }
            else if (hasMentionedSecrets && speech.Contains("journey"))
            {
                Say("The journey to uncovering Mondain's secrets is fraught with challenges. It requires patience and perseverance.");
            }
            else if (hasMentionedSecrets && speech.Contains("truth"))
            {
                Say("The truth is elusive and often hidden behind layers of deception. Seek with an open mind and a determined spirit.");
            }
            else if (hasMentionedSecrets && speech.Contains("persistent"))
            {
                Say("Persistence is key to uncovering the deepest secrets. Many give up too soon, but you have persevered.");
            }

            base.OnSpeech(e);
        }

        public RoderickNightshade(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(hasRevealedSecrets);
            writer.Write(hasMentionedDarkness);
            writer.Write(hasMentionedMondain);
            writer.Write(hasMentionedReveal);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            hasRevealedSecrets = reader.ReadBool();
            hasMentionedDarkness = reader.ReadBool();
            hasMentionedMondain = reader.ReadBool();
            hasMentionedReveal = reader.ReadBool();
        }
    }
}
