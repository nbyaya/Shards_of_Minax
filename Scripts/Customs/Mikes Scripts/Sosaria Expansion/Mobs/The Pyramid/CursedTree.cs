using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cursed tree corpse")]
    public class CursedTree : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextSporeTime;
        private DateTime m_NextEntangleTime;
        private DateTime m_NextBarkTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // Unique dark‑green hue for the Cursed Tree
        private const int UniqueHue = 1175;

        [Constructable]
        public CursedTree()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cursed tree";
            Body = 301;
            Hue = UniqueHue;
            BaseSoundID = 443;

            // —— Stats —— 
            SetStr(2000, 2300);
            SetDex(400, 550);
            SetInt(1000, 1200);

            SetHits(2000, 2400);
            SetStam(500, 650);
            SetMana(800, 1000);

            SetDamage(50, 60);
            SetDamageType(ResistanceType.Physical, 100);

            // —— Resistances —— 
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     80, 90);
            SetResistance(ResistanceType.Poison,   90,100);
            SetResistance(ResistanceType.Energy,   60, 70);

            // —— Skills —— 
            SetSkill(SkillName.Tactics,       100.1, 110.0);
            SetSkill(SkillName.Wrestling,     110.1, 120.0);
            SetSkill(SkillName.MagicResist,   120.1, 130.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 100;
            ControlSlots = 5;

            // —— Ability cooldowns —— 
            m_NextSporeTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextEntangleTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            m_NextBarkTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            m_NextSummonTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));

            m_LastLocation = this.Location;

            // —— Loot —— 
            PackItem(new Log(Utility.RandomMinMax(40, 60)));
            PackGold(1500, 2000);
        }

        public override int GetIdleSound()   { return 443; }
        public override int GetAttackSound() { return 672; }
        public override int GetDeathSound()  { return 31;  }

        // —— Life‑Drain Aura when foes move close —— 
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == this.Map && Alive && m.Alive && InRange(m.Location, 2) && CanBeHarmful(m, false))
            {
                // Ensure we only access Mobile properties when valid
                DoHarmful(m);

                int drain = Utility.RandomMinMax(20, 30);
                m.Damage(drain, this);
                this.Heal(drain);

                // Visual feedback
                m.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                PlaySound(0x1F8);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Track movement for potential future ground effects
            if (Location != m_LastLocation)
                m_LastLocation = Location;

            // Must be alive, in-world, and have a Mobile combatant
            if (!Alive || Map == null || Map == Map.Internal || !(Combatant is Mobile target))
                return;

            var now = DateTime.UtcNow;
            // — FIXED: call GetDistanceToSqrt on 'this' (a Mobile), passing the target Mobile
            int range = (int)this.GetDistanceToSqrt(target);

            // Poison Spore Burst
            if (now >= m_NextSporeTime && range <= 8)
            {
                PoisonSporeBurst();
                m_NextSporeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            // Entangling Roots
            else if (now >= m_NextEntangleTime && range <= 6)
            {
                EntangleRoots();
                m_NextEntangleTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Bark Shard Volley
            else if (now >= m_NextBarkTime && range <= 12)
            {
                BarkShardVolley();
                m_NextBarkTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Summon Vine Horrors
            else if (now >= m_NextSummonTime)
            {
                SummonVineHorrors();
                m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60));
            }
        }

        // —— Poison Spore Burst: drops PoisonTiles in AoE —— 
        private void PoisonSporeBurst()
        {
            Say("*The earth... corrupts!*");
            PlaySound(0x228);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 12, 60, UniqueHue, 0, 5039, 0);

            for (int dx = -2; dx <= 2; dx++)
            for (int dy = -2; dy <= 2; dy++)
            {
                Point3D p = new Point3D(X + dx, Y + dy, Z);
                if (Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                {
                    PoisonTile tile = new PoisonTile { Hue = UniqueHue };
                    tile.MoveToWorld(p, Map);
                }
            }
        }

        // —— Entangling Roots: TrapWeb at target location —— 
        private void EntangleRoots()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Roots... bind you!*");
            PlaySound(0x22F);

            Point3D p = target.Location;
            if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                p.Z = Map.GetAverageZ(p.X, p.Y);

            TrapWeb web = new TrapWeb { Hue = UniqueHue };
            web.MoveToWorld(p, Map);
        }

        // —— Bark Shard Volley: ranged pure-physical bolts with knockback —— 
        private void BarkShardVolley()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*Feel the thorns!*");
            PlaySound(0x20A);

            var targets = new List<Mobile> { initial };
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 10);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && initial.InLOS(m))
                    targets.Add(m);
                if (targets.Count >= 5) break;
            }
            eable.Free();

            foreach (var m in targets)
            {
                DoHarmful(m);
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, Map),
                    new Entity(Serial.Zero, m.Location, Map),
                    0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

                int dmg = Utility.RandomMinMax(40, 60);
                m.Damage(dmg, this);
                m.PlaySound(0x54);

                // — FIXED: Move only takes one argument now
                m.Move(Direction.Down);
            }
        }

        // —— Summon 1–3 Feral Treefellows as Vine Horrors —— 
        private void SummonVineHorrors()
        {
            Say("*Rise... my children!*");
            PlaySound(0x23D);

            int count = Utility.RandomMinMax(1, 3);
            for (int i = 0; i < count; i++)
            {
                Point3D p = new Point3D(
                    X + Utility.RandomList(-1, 1),
                    Y + Utility.RandomList(-1, 1),
                    Z);

                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = Map.GetAverageZ(p.X, p.Y);

                var minion = new FeralTreefellow
                {
                    Hue  = UniqueHue,
                    Team = this.Team
                };
                minion.MoveToWorld(p, Map);
            }
        }

        // —— On death: scatter Quicksand & Poison hazards —— 
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (Map == null)
				return;

            Say("*My roots... return to earth...*");
            PlaySound(0x214);

            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                Point3D p = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = Map.GetAverageZ(p.X, p.Y);

                Item tile = (i % 2 == 0)
                  ? (Item)new QuicksandTile()
                  : (Item)new PoisonTile();

                tile.Hue = UniqueHue;
                tile.MoveToWorld(p, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            PackItem(new Log(Utility.RandomMinMax(20, 40)));
        }

        public override bool BleedImmune => true;

        public CursedTree(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑sync cooldowns on load
            m_NextSporeTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextEntangleTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            m_NextBarkTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            m_NextSummonTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_LastLocation      = this.Location;
        }
    }
}
