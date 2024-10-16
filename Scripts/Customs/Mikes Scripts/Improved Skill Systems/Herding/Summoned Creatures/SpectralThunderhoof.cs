using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a spectral thunderhoof corpse")]
    public class SpectralThunderhoof : BaseCreature
    {
        private DateTime m_NextStampede;

        [Constructable]
        public SpectralThunderhoof()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spectral thunderhoof";
            Body = 0xE8;
            BaseSoundID = 0x64;
            Hue = 1153; // Ghostly blue hue

            SetStr(150, 200);
            SetDex(90, 120);
            SetInt(30, 50);

            SetHits(200, 250);
            SetMana(50, 75);

            SetDamage(12, 18);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 35, 45);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.MagicResist, 75.0, 90.0);
            SetSkill(SkillName.Tactics, 80.0, 95.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextStampede = DateTime.UtcNow;
        }

        public SpectralThunderhoof(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 0; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool BleedImmune { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextStampede)
            {
                DoStampede();
            }
        }

        public void DoStampede()
        {
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = GetMobilesInRange(8);

            foreach (Mobile m in eable)
            {
                if (m != this && m.Alive && CanBeHarmful(m) && !m.IsDeadBondedPet)
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            if (targets.Count > 0)
            {
                PlaySound(0x52D); // Thunderous sound
                FixedEffect(0x37CC, 10, 30, 1160, 0); // Blue sparkle effect

                foreach (Mobile target in targets)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 50, 0, 0, 0, 50);

                    target.FixedParticles(0x374A, 10, 30, 5013, 1153, 0, EffectLayer.Waist);
                    target.PlaySound(0x213); // Hit sound
                }

                PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* The Spectral Thunderhoof summons a herd of ghostly bulls! *");
            }

            m_NextStampede = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
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

            m_NextStampede = DateTime.UtcNow;
        }
    }
}