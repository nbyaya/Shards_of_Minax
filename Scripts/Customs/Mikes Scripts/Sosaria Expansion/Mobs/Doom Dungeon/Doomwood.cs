using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a doomwood corpse")]
    public class Doomwood : BaseCreature
    {
        // Cooldowns
        private DateTime m_NextSporeTime;
        private DateTime m_NextRootTime;
        private DateTime m_NextDecayTime;
        private Point3D m_LastLocation;

        // Unique dark toxic green
        private const int UniqueHue = 1175;

        [Constructable]
        public Doomwood()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Doomwood";
            Body = 301;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(100, 120);
            SetInt(200, 250);

            SetHits(1000, 1200);
            SetStam(150, 180);
            SetMana(100, 150);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            // Resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 95.1, 105.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Poisoning, 100.1, 110.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initial cooldowns
            m_NextSporeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRootTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextDecayTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));

            PackItem(new Log(Utility.RandomMinMax(15, 25)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(10, 15)));    // thematic reagent
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));

            m_LastLocation = this.Location;
        }

        public override TribeType Tribe => TribeType.Fey;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;

        public override int GetIdleSound()   => 443;
        public override int GetDeathSound()  => 31;
        public override int GetAttackSound() => 672;

        // Rotting Aura: poisons nearby on movement
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    target.FixedParticles(0x375A, 10, 15, 5011, EffectLayer.Head);
                    target.PlaySound(0x231);
                    target.ApplyPoison(this, Poison.Lethal);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // Leave toxic spores behind
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                Point3D old = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    PoisonTile tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(old, this.Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            if (DateTime.UtcNow >= m_NextSporeTime && InRange(Combatant.Location, 8))
            {
                SporeCloud();
                m_NextSporeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (DateTime.UtcNow >= m_NextRootTime && InRange(Combatant.Location, 12))
            {
                RootSnare();
                m_NextRootTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            else if (DateTime.UtcNow >= m_NextDecayTime && InRange(Combatant.Location, 6))
            {
                CursedDecay();
                m_NextDecayTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // AoE poison burst
        private void SporeCloud()
        {
            PlaySound(0x217);
            FixedParticles(0x3709, 20, 30, 5012, UniqueHue, 0, EffectLayer.CenterFeet);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile tgt)
                    list.Add(tgt);
            }

            foreach (Mobile tgt in list)
            {
                DoHarmful(tgt);
                int dmg = Utility.RandomMinMax(40, 60);
                AOS.Damage(tgt, this, dmg, 0, 0, 0, 0, 100);
                tgt.ApplyPoison(this, Poison.Lethal);
                tgt.FixedParticles(0x370A, 10, 15, 5011, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // Roots at target location
        private void RootSnare()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*The forest binds you!*");
                PlaySound(0x55E);

                var loc = target.Location;
                TrapWeb tile = new TrapWeb();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, this.Map);
            }
        }

        // Life-leech strike
        private void CursedDecay()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Drink of my rot!*");
                PlaySound(0x23D);

                DoHarmful(target);
                int damage = Utility.RandomMinMax(50, 75);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                target.FixedParticles(0x376A, 10, 15, 5014, UniqueHue, 0, EffectLayer.Head);

                // Heal self
                int heal = damage / 2;
                this.Heal(heal);
                this.FixedParticles(0x376A, 10, 15, 5013, UniqueHue, 0, EffectLayer.CenterFeet);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            // Poison pools around corpse
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                PoisonTile pt = new PoisonTile();
                pt.Hue = UniqueHue;
                pt.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));

            if (Utility.RandomDouble() < 0.03) // 3% chance
                PackItem(new HelmOfTheUnyieldingStar()); // unique crafting reagent
        }

        public Doomwood(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            m_NextSporeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRootTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextDecayTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));

            m_LastLocation = this.Location;
        }
    }
}
