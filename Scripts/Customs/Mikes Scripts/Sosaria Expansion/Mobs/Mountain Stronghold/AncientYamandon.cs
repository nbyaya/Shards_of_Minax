using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an ancient yamandon corpse")]
    public class AncientYamandon : BaseCreature
    {
        private DateTime m_NextStompTime;
        private DateTime m_NextShardTime;
        private DateTime m_NextMiasmaTime;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1890; // Deep obsidian-gray with fiery veins

        [Constructable]
        public AncientYamandon()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.25, 0.5)
        {
            Name = "an ancient yamandon";
            Body = 249;
            BaseSoundID = 1261;
            Hue = UniqueHue;

            // Stats
            SetStr(900, 1050);
            SetDex(200, 300);
            SetInt(120, 150);

            SetHits(2000, 2400);
            SetStam(300, 400);
            SetMana(150, 200);

            SetDamage(25, 45);

            // Damage Types
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Poison, 20);

            // Resistances
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 30, 50);

            // Skills
            SetSkill(SkillName.Anatomy,      120.1, 140.0);
            SetSkill(SkillName.Tactics,      120.1, 140.0);
            SetSkill(SkillName.Wrestling,    115.1, 135.0);
            SetSkill(SkillName.MagicResist,  125.1, 145.0);
            SetSkill(SkillName.Poisoning,    110.1, 130.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Ability cooldowns
            m_NextStompTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackItem(new Hides(20));
        }

        // Earthquake Stomp
        public void SeismicStomp()
        {
            if (Map == null) return;

            PlaySound(0x5A);
            FixedParticles(0x375A, 10, 20, 5032, UniqueHue, 0, EffectLayer.Waist);

            var targets = new List<Mobile>();
            var eable = Map.GetMobilesInRange(Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && this.CanBeHarmful(m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (Map == null) return;
                var tile = new EarthquakeTile { Hue = UniqueHue };
                tile.MoveToWorld(Location, Map);
            });
        }

        // Rock Shard Barrage
        public void ShardBarrage()
        {
            if (Map == null || !(Combatant is Mobile initialTarget))
                return;

            Say("The mountain obeys!");
            PlaySound(0x2A);

            var targets = new List<Mobile> { initialTarget };
            var eable = Map.GetMobilesInRange(initialTarget.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && this.CanBeHarmful(m) && !targets.Contains(m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile m in targets)
            {
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, Location, Map),
                    new Entity(Serial.Zero, m.Location, Map),
                    0x1BFB, 7, 0, false, false, UniqueHue, 0, 9503, 1, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.1), () =>
                {
                    if (this.CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 100, 0, 0, 0, 0);
                    }
                });
            }
        }

        // Poisonous Miasma
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != null && m != this && m.Map == this.Map && m.InRange(Location, 2) && m.Alive && this.CanBeHarmful(m))
            {
                m.SendMessage(0x22, "Toxic vapors choke you!");
                m.ApplyPoison(this, Poison.Lethal);
            }

            // Leave creeping poison cloud
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var cloud = new PoisonTile { Hue = UniqueHue };
                cloud.MoveToWorld(m_LastLocation, Map);
            }

            m_LastLocation = Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            if (now >= m_NextStompTime && InRange(Combatant.Location, 6))
            {
                SeismicStomp();
                m_NextStompTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextShardTime && InRange(Combatant.Location, 10))
            {
                ShardBarrage();
                m_NextShardTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextMiasmaTime)
            {
                var pool = new PoisonTile { Hue = UniqueHue };
                pool.MoveToWorld(Location, Map);
                PlaySound(0x3D);
                m_NextMiasmaTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // Counterpoison Burst when hit
        private void DoCounterBurst(Mobile attacker)
        {
            if (attacker != null && attacker.Alive && this.CanBeHarmful(attacker) && Utility.RandomDouble() < 0.15)
            {
                Animate(10, 5, 1, true, false, 0);
                attacker.FixedParticles(0x36BD, 1, 10, 0x1F78, UniqueHue, 0, EffectLayer.Head);
                attacker.ApplyPoison(this, Poison.Deadly);
                AOS.Damage(attacker, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 100);
            }
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            base.OnDamagedBySpell(caster);
            DoCounterBurst(caster);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);
            DoCounterBurst(attacker);
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The mountain trembles...*");
                Effects.PlaySound(Location, Map, 0x5A);

                for (int i = 0; i < 6; i++)
                {
                    var loc = new Point3D(
                        X + Utility.RandomMinMax(-3, 3),
                        Y + Utility.RandomMinMax(-3, 3),
                        Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var quake = new EarthquakeTile { Hue = UniqueHue };
                    quake.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // Sounds & immunities
        public override int GetAttackSound() => 1260;
        public override int GetAngerSound()  => 1262;
        public override int GetDeathSound()  => 1259;
        public override int GetHurtSound()   => 1263;
        public override int GetIdleSound()   => 1261;

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override int Hides => 25;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 8);

            if (Utility.RandomDouble() < 0.015)
                PackItem(new YamandonLordsStoneChest());
        }

        public AncientYamandon(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            m_NextStompTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation   = this.Location;
        }
    }
}
