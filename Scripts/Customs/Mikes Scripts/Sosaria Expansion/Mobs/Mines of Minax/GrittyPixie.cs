using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a gritty pixie corpse")]
    public class GrittyPixie : BaseCreature
    {
        // Cooldowns for special attacks
        private DateTime m_NextDustCloud;
        private DateTime m_NextShardBarrage;
        private DateTime m_NextQuicksand;
        private DateTime m_NextCamo;

        // Last known location for movement aura
        private Point3D m_LastLocation;

        // A desolate slate‑gray hue
        private const int GritHue = 1175;

        [Constructable]
        public GrittyPixie()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a gritty pixie";
            Body = 128;
            BaseSoundID = 0x467;
            Hue = GritHue;

            // Beefed‑up stats
            SetStr(150, 200);
            SetDex(400, 450);
            SetInt(300, 350);

            SetHits(800, 950);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(20, 30);

            // Damage profile
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Skills
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Hiding, 80.0, 90.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 70;
            ControlSlots = 4;

            // Initialize ability timers
            var now = DateTime.UtcNow;
            m_NextDustCloud     = now + TimeSpan.FromSeconds(8);
            m_NextShardBarrage  = now + TimeSpan.FromSeconds(12);
            m_NextQuicksand     = now + TimeSpan.FromSeconds(15);
            m_NextCamo          = now + TimeSpan.FromSeconds(20);

            m_LastLocation = this.Location;

            // Basic loot
            PackItem(new SulfurousAsh(10));
            PackItem(new SpidersSilk(10));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Gritty Aura: slow & minor poison if they step too close
            if (m != this && m.Map == this.Map &&
                m.InRange(this.Location, 2) && Alive && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // Apply poison
                    target.ApplyPoison(this, Poison.Deadly);

                    // Slow effect
                    target.SendMessage("You slog through gritty dust and feel slowed!");
                    AOS.Damage(target, this, 5, 0, 0, 100, 0, 0); // minor poison‑themed hit
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            var now = DateTime.UtcNow;

            // Update location for trail logic
            if (this.Location != m_LastLocation)
            {
                m_LastLocation = this.Location;
            }

            // No target, no special attacks
            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Quicksand Snare: ensnare the current target
            if (now >= m_NextQuicksand && Combatant is Mobile targetSnare && CanBeHarmful(targetSnare, false))
            {
                QuicksandSnare(targetSnare);
                m_NextQuicksand = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
                return;
            }

            // Shard Barrage: hail of razor‑sharp shale
            if (now >= m_NextShardBarrage && Combatant is Mobile targetShard && InRange(targetShard, 10))
            {
                ShardBarrage(targetShard);
                m_NextShardBarrage = now + TimeSpan.FromSeconds(Utility.RandomMinMax(14, 18));
                return;
            }

            // Dust Cloud: area‑of‑effect poison cloud around self
            if (now >= m_NextDustCloud)
            {
                DustCloud();
                m_NextDustCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
                return;
            }

            // Camouflage: vanish into the mine’s shadows
            if (now >= m_NextCamo && Utility.RandomDouble() < 0.3)
            {
                EnterCamo();
                m_NextCamo = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
        }

        // --- Special Abilities ---

        private void DustCloud()
        {
            this.Say("*Cough…*");
            PlaySound(0x64A); // choking dust sound

            // Create a ring of poison tiles around self
            for (int dx = -2; dx <= 2; dx++)
            {
                for (int dy = -2; dy <= 2; dy++)
                {
                    if (Math.Abs(dx) == 2 || Math.Abs(dy) == 2)
                    {
                        var loc = new Point3D(X + dx, Y + dy, Z);
                        if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        {
                            var tile = new PoisonTile();
                            tile.Hue = GritHue;
                            tile.MoveToWorld(loc, Map);
                        }
                    }
                }
            }
        }

        private void ShardBarrage(Mobile target)
        {
            if (!InRange(target, 12)) return;

            this.Say("Feel these shards of the deep!");
            PlaySound(0x307); // rock‐shattering sound

            // Fire 6 shards in quick succession
            for (int i = 0; i < 6; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (target.Alive && CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        Effects.SendMovingParticles(
                            new Entity(Serial.Zero, this.Location, this.Map),
                            new Entity(Serial.Zero, target.Location, target.Map),
                            0x1BFB,  // shard graphic
                            5, 0, false, false, GritHue, 0, 5035, 0, 0, EffectLayer.Waist, 0x100
                        );

                        int dmg = Utility.RandomMinMax(25, 35);
                        AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0); // 100% physical
                    }
                });
            }
        }

        private void QuicksandSnare(Mobile target)
        {
            this.Say("Sink… in the muck!");
            PlaySound(0x13E); // suction sound

            var loc = target.Location;
            if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);

            var qs = new QuicksandTile();
            qs.Hue = GritHue;
            qs.MoveToWorld(loc, Map);
        }

        private void EnterCamo()
        {
            this.Say("*…*");
            // Temporarily hide self
            this.Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(8), () => { if (Alive) this.Hidden = false; });
        }

        // --- Death Override ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            this.Say("…buried… forever…");
            PlaySound(0x65A); // collapsing dust
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 5, 20, GritHue, 0, 5029, 0
            );
        }

        // --- Loot & Properties ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 1);
            AddLoot(LootPack.Gems, 5);
            AddLoot(LootPack.MedScrolls, Utility.Random(2, 3));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new ShadebindWrap()); // your custom reagent
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus    => 60.0;

        public GrittyPixie(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            
            // Refresh timers on reload
            var now = DateTime.UtcNow;
            m_NextDustCloud    = now + TimeSpan.FromSeconds(10);
            m_NextShardBarrage = now + TimeSpan.FromSeconds(12);
            m_NextQuicksand    = now + TimeSpan.FromSeconds(15);
            m_NextCamo         = now + TimeSpan.FromSeconds(20);
        }
    }
}
