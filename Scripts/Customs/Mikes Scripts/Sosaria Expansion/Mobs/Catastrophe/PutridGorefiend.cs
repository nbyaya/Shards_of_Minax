using System;
using Server;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a putrid gorefiend corpse")]
    public class PutridGorefiend : BaseCreature
    {
        [Constructable]
        public PutridGorefiend() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Putrid Gorefiend";
            Body = 305;
            Hue = 0x3F; // Sickly green hue
            BaseSoundID = 224;

            SetStr(350, 400);
            SetDex(80, 120);
            SetInt(150, 200);

            SetHits(800, 1000);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Poisoning, 120.0);
            SetSkill(SkillName.Necromancy, 80.0);

            Fame = 8500;
            Karma = -8500;

            VirtualArmor = 50;

            // Unique abilities
            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public PutridGorefiend(Serial serial) : base(serial)
        {
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Deadly;
        public override bool AutoDispel => true;
        public override bool Unprovokable => true;

        private DateTime _nextPlagueCloud;
        private DateTime _nextBloodBoil;

        public override void OnActionCombat()
        {
            if (DateTime.UtcNow >= _nextPlagueCloud)
            {
                PlagueCloud();
                _nextPlagueCloud = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }

            if (DateTime.UtcNow >= _nextBloodBoil && 0.5 > Utility.RandomDouble())
            {
                BloodBoil();
                _nextBloodBoil = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }

            base.OnActionCombat();
        }

        private void PlagueCloud()
        {
            if (Combatant is Mobile target)
            {
                this.Say("*A noxious cloud erupts from the gorefiend's wounds!*");
                new InternalPoisonCloud(this, target.Location).Start();
            }
        }

        private void BloodBoil()
        {
            if (Combatant is Mobile target)
            {
                this.Say("*Your blood burns with unholy fire!*");
                target.SendMessage("Your veins burn with corrupted blood!");
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                target.PlaySound(0x307);

                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    if (target.Alive)
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
                        target.Stam -= 50;
                        target.Mana -= 50;
                        target.ApplyPoison(this, Poison.Deadly);
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            // Putrid Explosion
            this.Say("*The gorefiend's corpse erupts in a wave of decay!*");
            foreach (Mobile m in this.GetMobilesInRange(5))
            {
                if (m != this && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(50, 80), 0, 100, 0, 0, 0);
                    m.ApplyPoison(this, Poison.Lethal);
                    m.SendMessage("You're drenched in toxic viscera!");
                }
            }
            
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss);
            
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

        private class InternalPoisonCloud : Timer
        {
            private readonly Mobile m_Owner;
            private readonly Point3D m_Location;
            private int m_Count;

            public InternalPoisonCloud(Mobile owner, Point3D loc) : base(TimeSpan.FromSeconds(1.5), TimeSpan.FromSeconds(3.0))
            {
                m_Owner = owner;
                m_Location = loc;
            }

            protected override void OnTick()
            {
                if (m_Count++ == 5)
                    return;

                foreach (Mobile m in m_Owner.GetMobilesInRange(3))
                {
                    if (m != m_Owner && m_Owner.CanBeHarmful(m))
                    {
                        m_Owner.DoHarmful(m);
                        AOS.Damage(m, m_Owner, Utility.RandomMinMax(20, 40), 0, 100, 0, 0, 0);
                        m.ApplyPoison(m_Owner, Poison.Greater);
                    }
                }
            }
        }
    }

    public class RingOfFesteringSouls : BaseRing
    {
        [Constructable]
        public RingOfFesteringSouls() : base(0x108A)
        {
            Hue = 0x3F;
            Attributes.RegenHits = 3;
            Attributes.WeaponDamage = 25;
            Resistances.Poison = 30;
        }

        public RingOfFesteringSouls(Serial serial)
            : base(serial)
        {
        }

        //... (standard serialization code)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }		
    }

    public class PutridFlesh : Item
    {
        [Constructable]
        public PutridFlesh() : base(0x318D)
        {
            Stackable = true;
            Hue = 0x3F;
            Name = "Putrid Flesh";
        }

        public PutridFlesh(Serial serial)
            : base(serial)
        {
        }

        //... (standard serialization code)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }		
    }
}