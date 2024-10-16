using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a fire clan ninja")]
    public class FireClanNinja : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0); // shorter time between fire ninja speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public FireClanNinja() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Setting the hue to a fire-related color to fit the ninja theme
            Hue = 0x489; // Choose a fire-like color

            // Setting the body to human, ninjas are typically human
            Body = 0x190; // Use 0x191 for a female ninja

            Name = NameList.RandomName("male");
            Title = "the Inferno Shadow";

		
			// Dressing the ninja in a blue ninja outfit
            AddItem(new NinjaTabi() { Hue = 0x489 }); // Blue ninja sandals
            AddItem(new LeatherNinjaHood() { Hue = 0x489 }); // Blue ninja hood
            AddItem(new LeatherNinjaJacket() { Hue = 0x489 }); // Blue ninja jacket
            AddItem(new LeatherNinjaPants() { Hue = 0x489 }); // Blue ninja pants
            AddItem(new LeatherNinjaBelt() { Hue = 0x489 }); // Blue ninja belt

            // Defining stats for a fire clan ninja, focused on strength and ferocity
            SetStr(800, 950);
            SetDex(200, 650);
            SetInt(100, 400);
            SetHits(800, 1500);

            SetDamage(130, 140);

            // Ninjas should have high stealth and poisoning skills
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
			SetSkill(SkillName.Fencing, 120.0, 200.0);
            SetSkill(SkillName.Macing, 120.0, 200.0);
            SetSkill(SkillName.MagicResist, 150.0, 220.0);
            SetSkill(SkillName.Swords, 120.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 200.0);
            SetSkill(SkillName.Wrestling, 120.0, 200.0);
            SetSkill(SkillName.Ninjitsu, 120.0, 200.0); // If your shard has the Ninjitsu skill

            // Karma for a fire clan ninja might be more negative than a green ninja
            Fame = 15000;
            Karma = -5000;

            // Implementing ninja speech patterns, fiery and aggressive
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
                        case 0: this.Say(true, "The flames shall consume you..."); break;
                        case 1: this.Say(true, "Feel the scorching heat of my wrath!"); break;
                        case 2: this.Say(true, "From the ashes of my enemies, I rise!"); break;
                        case 3: this.Say(true, "You will not withstand the inferno!"); break;
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
						case 0: this.Say(true, "The flames shall consume you..."); break;
                        case 1: this.Say(true, "Feel the scorching heat of my wrath!"); break;
                        case 2: this.Say(true, "From the ashes of my enemies, I rise!"); break;
                        case 3: this.Say(true, "You will not withstand the inferno!"); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
				}
			}

			return base.Damage(amount, from);
		}

        // Implementing the loot generation, fire clan ninjas might carry special fire-related loot
        public override void GenerateLoot()
        {
            PackGold(250, 500);
			AddLoot(LootPack.UltraRich);  // Even richer loot than before
            // Add specific fire-themed ninja loot, like fire bombs or oil flasks
        }

        public FireClanNinja(Serial serial) : base(serial)
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
