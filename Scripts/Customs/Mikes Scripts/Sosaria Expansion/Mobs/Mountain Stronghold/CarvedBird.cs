using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shattered stone bird corpse")]
    public class CarvedBird : BaseCreature
    {
        private DateTime m_NextScreech;
        private DateTime m_NextGust;
        private DateTime m_NextShardStorm;
        private Point3D m_LastLocation;

        // Unique gray‑stone hue
        private const int UniqueHue = 2101;

        [Constructable]
        public CarvedBird() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 2, 0.2, 0.4)
        {
            Name = "a carved bird";
            Body = 5;            // Macaw body
            Hue = UniqueHue;
            BaseSoundID = 0x2EF; // Macaw idle sound

            // Stats
            SetStr(300, 350);
            SetDex(300, 350);
            SetInt(200, 250);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(350, 450);

            SetDamage(20, 30);

            // Damage profile
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 25);
            SetDamageType(ResistanceType.Cold, 25);

            // Resistances
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     70, 80);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   60, 70);

            // Skills
            SetSkill(SkillName.Wrestling,   100.0, 110.0);
            SetSkill(SkillName.Tactics,     100.0, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.EvalInt,     90.0, 100.0);
            SetSkill(SkillName.Magery,      90.0, 100.0);
            SetSkill(SkillName.Meditation,  80.0,  90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4; // boss-level

            // Initialize cooldowns
            m_NextScreech   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextGust      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShardStorm= DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Basic loot
            PackItem(new IronIngot(Utility.RandomMinMax(5, 10)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
        }

        public CarvedBird(Serial serial)
            : base(serial)
        {
        }

        // Macaw sounds
        public override int GetIdleSound()  { return 0x2EF; }
        public override int GetAttackSound(){ return 0x2EE; }
        public override int GetAngerSound() { return 0x2EF; }
        public override int GetHurtSound()  { return 0x2F1; }
        public override int GetDeathSound() { return 0x2F2; }

        // Movement‑based hazard: leave razor‑feather landmines
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (Deleted || !Alive) return;

            if (m != this && m.Map == Map && m.InRange(Location, 2) && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target && Utility.RandomDouble() < 0.20)
                {
                    var loc = target.Location;
                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var tile = new LandmineTile { Hue = UniqueHue };
                        tile.MoveToWorld(loc, Map);
                    }
                }
            }
        }

        // Core AI tick: check ability cooldowns
        public override void OnThink()
        {
            base.OnThink();

            if (Deleted || !Alive || Map == null || Map == Map.Internal) return;
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                if (DateTime.UtcNow >= m_NextScreech && InRange(target.Location, 8))
                {
                    SonicScreech();
                    m_NextScreech = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                }
                else if (DateTime.UtcNow >= m_NextGust && InRange(target.Location, 10))
                {
                    MountainGust(target);
                    m_NextGust = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
                else if (DateTime.UtcNow >= m_NextShardStorm)
                {
                    ShardStorm();
                    m_NextShardStorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
            }
        }

        // 1) Sonic Screech — AoE physical → stun
        private void SonicScreech()
        {
            Say("*SCREEEAAAK!*");
            PlaySound(0x2EE);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
                if (m != this && m.Alive && CanBeHarmful(m, false))
                    list.Add(m);

            foreach (Mobile m in list)
            {
                DoHarmful(m);
                if (m is Mobile tgt)
                {
                    tgt.SendMessage("The deafening screech rattles your head!");
                    int dmg = Utility.RandomMinMax(30, 45);
                    AOS.Damage(tgt, this, dmg, 0,0,0,0,100);
                    // brief stun
                    tgt.Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => { if (!tgt.Deleted) tgt.Frozen = false; });
                }
            }
        }

        // 2) Mountain Gust — single‑target push + slow
        private void MountainGust(Mobile target)
        {
            Say("*Wind of the peaks!*");
            PlaySound(0x1F8);

            Effects.SendMovingParticles(
                this, target, 0x51, 5, 0, false, true, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100
            );

            DoHarmful(target);
            target.SendMessage("A fierce gust pushes you off balance!");
            int dmg = Utility.RandomMinMax(20, 30);
            AOS.Damage(target, this, dmg, 50,0,50,0,0); // 50% Phys, 50% Cold
        }

        // 3) Shard Storm — AoE cold damage + spawns IceShardTiles
        private void ShardStorm()
        {
            Say("*Shards of the mountain!*");
            PlaySound(0x208);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
                if (m != this && m.Alive && CanBeHarmful(m, false))
                    list.Add(m);

            foreach (Mobile m in list)
            {
                DoHarmful(m);
                if (m is Mobile tgt)
                {
                    int dmg = Utility.RandomMinMax(40, 60);
                    AOS.Damage(tgt, this, dmg, 0,0,0,0,100);
                    tgt.FixedParticles(0x36BD, 20, 10, 5042, UniqueHue, 0, EffectLayer.Head);
                    PlaySound(0x31E);
                }
            }

            // litter the ground with icy hazards
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tile = new IceShardTile { Hue = UniqueHue };
                tile.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // Dramatic death: explosion + landmine spikes
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (this.Map == null)
				return;

            Say("*…broken…*");
            Effects.PlaySound(Location, Map, 0x2F2);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x36BD, 20, 10, UniqueHue, 0, 5042, 0
            );

            if (0.5 > Utility.RandomDouble())
                c.DropItem(new Granite(Utility.RandomMinMax(5, 15))); // stone fragments

            for (int i = 0; i < 4; i++)
            {
                int x = X + Utility.RandomMinMax(-2, 2);
                int y = Y + Utility.RandomMinMax(-2, 2);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var spike = new LandmineTile { Hue = UniqueHue };
                spike.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // Standard loot, serialization, etc.
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers
            m_NextScreech    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextGust       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShardStorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
