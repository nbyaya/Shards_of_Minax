using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a green ninja")]
    public class GreenNinja : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(30.0); // time between green ninja speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public GreenNinja() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Setting the hue to a green color to fit the ninja theme
            Hue = 0x851;
			Team = 1;

            // Setting the body to human, ninjas are typically human
            Body = 0x190; // Use 0x191 for a female ninja

            Name = NameList.RandomName("male");
            Title = "the Silent Blade";

            // Dressing the ninja in a green ninja outfit
            AddItem(new Robe(0x851)); // Green ninja sandals


            // Defining stats for a green ninja, focused on dexterity and agility
            SetStr(700, 850);
            SetDex(200, 600);
            SetInt(100, 600);
            SetHits(700, 1400);

            SetDamage(120, 130);

            // Ninjas should have high stealth and poisoning skills
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
			SetSkill(SkillName.Fencing, 120.0, 200.0);
            SetSkill(SkillName.Macing, 120.0, 200.0);
            SetSkill(SkillName.MagicResist, 120.0, 200.0);
            SetSkill(SkillName.Swords, 120.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 200.0);
            SetSkill(SkillName.Wrestling, 120.0, 200.0);
            SetSkill(SkillName.Ninjitsu, 120.0, 200.0); // If your shard has the Ninjitsu skill

            // Karma for a green ninja could be neutral or slightly negative
            Fame = 10000;
            Karma = -2000;

            // Implementing ninja speech patterns, mysterious and concise
            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Silence is death..."); break;
                        case 1: this.Say(true, "You cannot hide from the shadows."); break;
                        case 2: this.Say(true, "I strike from where you least expect."); break;
                        case 3: this.Say(true, "The last breath you take will be my doing."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
                }

                base.OnThink();
            }			
        }
		
		public override int Damage(int amount, Mobile from)
		{
			Mobile combatant = this.Combatant as Mobile;

			if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
			{
				if (Utility.RandomBool())
				{
					int phrase = Utility.Random(4);

					switch (phrase)
					{
						case 0: this.Say(true, "Silence is death..."); break;
                        case 1: this.Say(true, "You cannot hide from the shadows."); break;
                        case 2: this.Say(true, "I strike from where you least expect."); break;
                        case 3: this.Say(true, "The last breath you take will be my doing."); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
				}
			}

			return base.Damage(amount, from);
		}

        // Implementing the loot generation, ninjas might carry smoke bombs, shurikens, etc.
        public override void GenerateLoot()
        {
            PackGold(200, 300);
            // Add specific ninja-themed loot
        }

        public GreenNinja(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
