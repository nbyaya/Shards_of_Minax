using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of an earth clan samurai")]
    public class EarthClanSamurai : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(30.0); // Time between earth clan samurai's speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public EarthClanSamurai() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Earth Clan Samurai should have a grounded hue, representing their earthy nature
            Hue = 0x734; // Adjust the hue to fit your shard's earth theme
			Team = 1;

            // Body value should remain humanoid, representing the Samurai's discipline
            Body = 0x190; // Use the shard's specific body value for samurai if available
            Name = NameList.RandomName("male");
            Title = "the Earth Clan Protector"; // A title that denotes their role as a protector of the earth

            // Equip the Earth Clan Samurai with appropriate armor and weapons
            AddItem(new PlateDo());
            AddItem(new PlateHaidate());
			AddItem(new PlateSuneate());
            AddItem(new SamuraiTabi());
            AddItem(new NorseHelm());

            // Set stats and skills to reflect a masterful warrior
            SetStr(650, 800);
            SetDex(150, 450);
            SetInt(90, 400);
            SetHits(800, 1200);

            SetDamage(100, 120);

            SetSkill(SkillName.Bushido, 100.0, 150.0);
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
			SetSkill(SkillName.Fencing, 90.0, 120.0);
            SetSkill(SkillName.Macing, 90.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 150.0);
            SetSkill(SkillName.Swords, 110.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 150.0);
            SetSkill(SkillName.Wrestling, 100.0, 150.0);

            // Adjust the karma and fame to reflect their earthy stature
            Fame = 15000;
            Karma = 15000;

            // Earth Clan Samurai's speech could be wise and grounding
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
                        case 0: this.Say(true, "Feel the might of the earth!"); break;
                        case 1: this.Say(true, "I stand as unshakable as the mountains!"); break;
                        case 2: this.Say(true, "The land itself fights by my side!"); break;
                        case 3: this.Say(true, "You will be buried beneath the soil!"); break;
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
                        case 0: this.Say(true, "The earth will reclaim you!"); break;
                        case 1: this.Say(true, "You cannot move the mountain!"); break;
                        case 2: this.Say(true, "I am rooted, I cannot be overthrown!"); break;
                        case 3: this.Say(true, "Rocks shall break your bones!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        // Implementing the loot generation
        public override void GenerateLoot()
        {
            // Earth Clan Samurai might carry unique earth-themed items or artifacts
            PackGold(800, 1300);
            // Add earth-themed loot here
        }

        // ... Remaining methods can remain unaltered unless further customization is desired ...

        public EarthClanSamurai(Serial serial) : base(serial)
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
