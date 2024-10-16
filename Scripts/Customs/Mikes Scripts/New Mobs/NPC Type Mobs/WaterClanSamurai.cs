using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a water clan samurai")]
    public class WaterClanSamurai : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(30.0); // Time between water clan samurai's speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public WaterClanSamurai() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0x5; // A hue that suggests a connection to water, such as a deep blue

            Body = 0x190; // Standard humanoid body
            Name = NameList.RandomName("male");
            Title = "the Water Clan Protector";

            // Equip the Water Clan Samurai with water-themed armor and weapons
            AddItem(new PlateDo() { Hue = 0x5 }); // Water-colored Do
            AddItem(new PlateHaidate() { Hue = 0x5 }); // Water-colored Haidate
            AddItem(new PlateSuneate() { Hue = 0x5 }); // Water-colored Suneate
            AddItem(new SamuraiTabi() { Hue = 0x5 }); // Water-colored Tabi
            AddItem(new DecorativePlateKabuto() { Hue = 0x5 }); // Water-colored Kabuto

            SetStr(650, 800);
            SetDex(250, 350);
            SetInt(100, 150);
            SetHits(800, 1200);

            SetDamage(100, 120);

            SetSkill(SkillName.Bushido, 90.0, 120.0);
            SetSkill(SkillName.Anatomy, 90.0, 110.0);
            SetSkill(SkillName.Fencing, 90.0, 120.0);
            SetSkill(SkillName.Macing, 90.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 120.0);
            SetSkill(SkillName.Swords, 90.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 120.0);

            Fame = 15000;
            Karma = 15000;

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
                        case 0: this.Say(true, "As fluid as the tide!"); break;
                        case 1: this.Say(true, "Drown in the might of the Water Clan!"); break;
                        case 2: this.Say(true, "Flow like water, strike like the tsunami!"); break;
                        case 3: this.Say(true, "Your strategies are as shallow as a puddle!"); break;
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
                        case 0: this.Say(true, "The storm surge is upon you!"); break;
                        case 1: this.Say(true, "You cannot grasp the depths of my resolve!"); break;
                        case 2: this.Say(true, "I embody the relentless power of the seas!"); break;
                        case 3: this.Say(true, "You will be swept away!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        // Adjust the loot generation to include water-themed items
        public override void GenerateLoot()
        {
            PackGold(800, 1200);
            // Add water-themed loot here
        }

        // ... Remaining methods can remain unaltered unless further customization is desired ...

        public WaterClanSamurai(Serial serial) : base(serial)
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
