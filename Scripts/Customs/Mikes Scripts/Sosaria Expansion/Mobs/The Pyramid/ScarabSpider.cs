using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a scarab spider corpse")]
    public class ScarabSpider : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextAcidSpitTime;
        private DateTime m_NextWebTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // Unique golden hue
        private const int UniqueHue = 1234;

        [Constructable]
        public ScarabSpider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Scarab Spider";
            Body = 0x9d;
            BaseSoundID = 0x388;
            Hue = UniqueHue;

            // Significantly boosted stats
            SetStr(150, 175);
            SetDex(180, 200);
            SetInt(80, 90);

            SetHits(600, 650);
            SetStam(250, 300);
            SetMana(150, 200);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 55);
            SetResistance(ResistanceType.Fire, 35, 40);
            SetResistance(ResistanceType.Cold, 35, 40);
            SetResistance(ResistanceType.Poison, 80, 85);
            SetResistance(ResistanceType.Energy, 40, 45);

            // Skills
            SetSkill(SkillName.Anatomy, 100.0, 110.0);
            SetSkill(SkillName.Poisoning, 120.0, 125.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 130.0, 140.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize cooldowns
            m_NextAcidSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextWebTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummonTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Initial location tracker
            m_LastLocation = this.Location;

            // Basic loot
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 20)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
        }

        // Leave trap webs behind as it moves
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Alive && this.Map != null && this.Location != m_LastLocation)
            {
                // 30% chance to leave a web tile
                if (Utility.RandomDouble() < 0.30 && this.Map.CanFit(oldLocation.X, oldLocation.Y, oldLocation.Z, 16, false, false))
                {
                    var web = new TrapWeb();
                    web.Hue = UniqueHue;
                    web.MoveToWorld(oldLocation, this.Map);
                }

                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Acid Spit (short range cone)
            if (DateTime.UtcNow >= m_NextAcidSpitTime && this.InRange(Combatant.Location, 6))
            {
                AcidSpit();
                m_NextAcidSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            // Web Snare (mid-range root)
            else if (DateTime.UtcNow >= m_NextWebTime && this.InRange(Combatant.Location, 8))
            {
                WebSnare();
                m_NextWebTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            // Summon Scarabs (calls small minions)
            else if (DateTime.UtcNow >= m_NextSummonTime)
            {
                SummonScarabs();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Acid Spit: cone of acid and poison
        private void AcidSpit()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Sssccchh…*");
            PlaySound(0x1F3); // acidic spit sound

            // Particle effect toward target
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x36D4, 7, 0, false, true, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (!Alive || Map == null) return;

                var nearby = new List<Mobile>();
                foreach (var m in Map.GetMobilesInRange(this.Location, 4))
                {
                    if (m != this && m is Mobile m2 && CanBeHarmful(m2, false))
                        nearby.Add(m2);
                }

                foreach (var m2 in nearby)
                {
                    DoHarmful(m2);
                    int dmg = Utility.RandomMinMax(25, 40);
                    AOS.Damage(m2, this, dmg, 0, 0, 0, 50, 50); // 50% physical, 50% poison
                    m2.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            });
        }

        // --- Web Snare: roots the target and creates trap webs
        private void WebSnare()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Chkkk…*");
            PlaySound(0x2E5); // web sound

            // Create a cluster of webs around the target
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var loc = new Point3D(target.X + i, target.Y + j, target.Z);
                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var web = new TrapWeb();
                        web.Hue = UniqueHue;
                        web.MoveToWorld(loc, this.Map);
                    }
                }
            }

            // Apply a brief paralyze
            target.Paralyze(TimeSpan.FromSeconds(3.0));
            target.SendMessage(0x22, "You are entangled in sticky webs!");
        }

        // --- Summon Scarabs: spawn allied minions
        private void SummonScarabs()
        {
            Say("*Kchk-kchk-kchk!*");
            PlaySound(0x212); // skittering sound

            int count = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < count; i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var spawn = new Skeleton(); // Assume ScarabSwarm is defined elsewhere
                spawn.Team = this.Team;
                spawn.MoveToWorld(loc, this.Map);
                spawn.Combatant = this.Combatant;
            }
        }

        // On death, drop poisonous ground hazards
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            Say("*Ssstrrr…*");
            PlaySound(0x23A); // death hiss

            // Scatter poison tiles around the corpse
            int tiles = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < tiles; i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var poison = new PoisonTile(); // damaging poison ground
                poison.Hue = UniqueHue;
                poison.MoveToWorld(loc, this.Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 12));

            // Rare scarab idol drop
            if (Utility.RandomDouble() < 0.025) // 2.5% chance
            {
                PackItem(new StormweldBracers()); // assume defined elsewhere
            }
        }

        // Immunities & overrides
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 5; } }

        public ScarabSpider(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextAcidSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextWebTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummonTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
