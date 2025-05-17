using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a sangsleet corpse")]
    public class Sangsleet : BaseCreature
    {
        private DateTime m_NextBloodNova;
        private DateTime m_NextEssenceSiphon;

        [Constructable]
        public Sangsleet()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Sangsleet";
            Body = 303; 
            BaseSoundID = 357; 
            Hue = 1175;

            // Stats
            SetStr(400, 500);
            SetDex(200, 250);
            SetInt(350, 450);
            SetHits(8000, 9000);
            SetDamage(25, 35);

            // Damage types
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold,     20);
            SetDamageType(ResistanceType.Energy,   20);
            SetDamageType(ResistanceType.Poison,   30);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   75, 85);
            SetResistance(ResistanceType.Energy,   50, 60);

            // Skills
            SetSkill(SkillName.Necromancy,    100.0, 120.0);
            SetSkill(SkillName.SpiritSpeak,   100.0, 120.0);
            SetSkill(SkillName.EvalInt,       100.0, 120.0);
            SetSkill(SkillName.Magery,        100.0, 120.0);
            SetSkill(SkillName.Meditation,    100.0, 120.0);
            SetSkill(SkillName.MagicResist,   110.0, 130.0);
            SetSkill(SkillName.Tactics,       90.1, 100.0);
            SetSkill(SkillName.Wrestling,     100.1, 115.0);
            SetSkill(SkillName.DetectHidden,  90.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 60;

            PackNecroReg(50, 80);

            // These calls will compile now — see the stub methods below
            PackMagicEquipment(0.8, 0.4);
            PackSlayer();
        }

        public Sangsleet(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune => Poison.Lethal;
        public override bool BleedImmune => true;
        public override int Meat => 5;
        public override int TreasureMapLevel => 5;

        // Blood Nova AOE
        public void BloodNova()
        {
            if (DateTime.UtcNow < m_NextBloodNova)
                return;

            m_NextBloodNova = DateTime.UtcNow + TimeSpan.FromSeconds(15.0 + 10.0 * Utility.RandomDouble());

            Say("Feel the blood drain from your very essence!");
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 9, 32, 2, 0, 5042, 0);
            PlaySound(0x1EA);

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                var targets = new List<Mobile>();
                foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
                {
                    if (m is PlayerMobile && CanBeHarmful(m))
                    {
                        targets.Add(m);
                        DoHarmful(m);

                        Effects.SendLocationParticles(
                            EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                            0x3728, 10, 10, 2, 0, 5029, 0);
                        m.PlaySound(0x225);
                    }
                }

                foreach (Mobile m in targets)
                {
                    int damage = Utility.RandomMinMax(30, 50) + (Int / 20);
                    AOS.Damage(m, this, damage, 0, 0, 20, 20, 60);

                    if (Utility.RandomDouble() < 0.75)
                    {
                        int drain = Utility.RandomMinMax(10, 25);
                        m.Mana -= drain;
                        m.Stam -= drain;
                        m.SendAsciiMessage("You feel your energy rapidly drain away!");
                    }

                    if (Utility.RandomDouble() < 0.50)
                    {
                        m.ApplyPoison(this, Poison.Lesser);
                        m.SendAsciiMessage("You are afflicted by a sickly bleed!");
                    }
                }
            });
        }

        // Essence Siphon single‑target
        public void EssenceSiphon(Mobile target)
        {
            if (DateTime.UtcNow < m_NextEssenceSiphon)
                return;

            m_NextEssenceSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(8.0 + 4.0 * Utility.RandomDouble());
            DoHarmful(target);

            // Note the cast here — SendTargetParticles wants an int layer
            Effects.SendTargetParticles(
                target,
                0x374A, 10, 15,
                5013, 0,
                9502, 0,
                (int)EffectLayer.Waist);

            PlaySound(0x231);

            int drain = Utility.RandomMinMax(20, 40) + (Int / 30);
            int manaDrain = Math.Min(drain, target.Mana);
            int stamDrain = Math.Min(drain, target.Stam);
            int hitDrain  = Utility.RandomMinMax(drain / 4, drain / 2);

            target.Mana -= manaDrain;
            target.Stam -= stamDrain;
            AOS.Damage(target, this, hitDrain, 100, 0, 0, 0, 0);

            int healAmount = (int)((manaDrain + stamDrain + hitDrain) * 0.5);
            Heal(healAmount);

            target.SendAsciiMessage("Sangsleet siphons your essence!");
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant is Mobile target)
            {
                if (Utility.RandomDouble() <= 0.33)
                    EssenceSiphon(target);

                if (DateTime.UtcNow >= m_NextBloodNova)
                    BloodNova();
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.10)
                c.DropItem(new IronOre(Utility.RandomMinMax(5, 15)));

            // Replaced ManaPotion with RefreshPotion to match default ServUO
            if (Utility.RandomDouble() < 0.01)
            {
                for (int i = 0; i < 4; i++)
                    c.DropItem(new RefreshPotion());
            }

            c.DropItem(new Gold(5000, 10000));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.HighScrolls);
            AddLoot(LootPack.Gems);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(m_NextBloodNova);
            writer.Write(m_NextEssenceSiphon);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                m_NextBloodNova     = reader.ReadDateTime();
                m_NextEssenceSiphon = reader.ReadDateTime();
            }
            else
            {
                m_NextBloodNova     = DateTime.UtcNow;
                m_NextEssenceSiphon = DateTime.UtcNow;
            }
        }

        //
        // Stub methods to satisfy the missing PackMagicEquipment/PackSlayer calls
        //
        public void PackMagicEquipment(double armorChance, double weaponChance)
        {
            // Implement random magic armor/weapon here if you wish
        }

        public void PackSlayer()
        {
            // Implement slayer‐item packing here if you wish
        }
    }
}
