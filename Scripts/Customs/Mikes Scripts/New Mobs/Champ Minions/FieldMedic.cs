using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a field medic")]
    public class FieldMedic : BaseCreature
    {
        private TimeSpan m_HealDelay = TimeSpan.FromSeconds(10.0); // time between heals
        private DateTime m_NextHealTime;
        
        private TimeSpan m_ReviveDelay = TimeSpan.FromSeconds(30.0); // time between revives
        private DateTime m_NextReviveTime;

        [Constructable]
        public FieldMedic() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Field Medic";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Field Medic";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item robe = new Robe();
            Item boots = new Boots();

            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(robe);
            AddItem(boots);

            SetStr(250, 350);
            SetDex(150, 250);
            SetInt(300, 400);

            SetHits(300, 450);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.Healing, 90.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 70.1, 90.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

            Fame = 5000;
            Karma = 5000;

            VirtualArmor = 40;

            m_NextHealTime = DateTime.Now + m_HealDelay;
            m_NextReviveTime = DateTime.Now + m_ReviveDelay;
        }

        public override void OnThink()
        {
            base.OnThink();
            
            if (DateTime.Now >= m_NextHealTime)
            {
                HealAllies();
                m_NextHealTime = DateTime.Now + m_HealDelay;
            }

            if (DateTime.Now >= m_NextReviveTime)
            {
                ReviveComrades();
                m_NextReviveTime = DateTime.Now + m_ReviveDelay;
            }
        }

        private void HealAllies()
        {
            foreach (Mobile ally in this.GetMobilesInRange(5))
            {
                if (ally != this && ally is BaseCreature && !ally.Deleted && ally.Alive && ally.Hits < ally.HitsMax)
                {
                    BaseCreature creature = (BaseCreature)ally;
                    creature.Heal(Utility.RandomMinMax(10, 30));
                    creature.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                    creature.PlaySound(0x1F2);
                }
            }
        }

        private void ReviveComrades()
        {
            foreach (Mobile ally in this.GetMobilesInRange(5))
            {
                if (ally != this && ally is BaseCreature && !ally.Deleted && !ally.Alive)
                {
                    ally.Resurrect();
                    ally.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                    ally.PlaySound(0x214);
                }
            }
        }

        public FieldMedic(Serial serial) : base(serial)
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
