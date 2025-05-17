using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("the shattered nexus corpse")]
    public class NexusWeaver : BaseCreature
    {
        private int _stage;
        private bool _inTransition;
        private DateTime _nextAbility;

        [Constructable]
        public NexusWeaver()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Azura the Nexus Weaver";
            Body = 400;
            Hue = 1157;
            BaseSoundID = 0xA5;

            SetStr(1000, 1200);
            SetDex(150, 200);
            SetInt(900, 1100);

            SetHits(5000, 6000);
            SetStam(300, 400);
            SetMana(2000, 2500);

            SetDamage(30, 50);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire,     40);
            SetDamageType(ResistanceType.Cold,     40);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     60, 70);
            SetResistance(ResistanceType.Cold,     60, 70);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   60, 70);

            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Tactics,   120.0);
            SetSkill(SkillName.MagicResist,150.0);
            SetSkill(SkillName.Magery,    120.0);
            SetSkill(SkillName.EvalInt,   120.0);

            Fame = 24000;
            Karma = -24000;

            _stage        = 1;
            _inTransition = false;
            _nextAbility  = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Deleted || !Alive)
                return;

            CheckStageTransition();

            if (!_inTransition && DateTime.UtcNow >= _nextAbility)
            {
                PerformStageAbility();
                _nextAbility = DateTime.UtcNow + TimeSpan.FromSeconds(_stage < 4 ? 10 : 7);
            }
        }

        private void CheckStageTransition()
        {
            double pct = (double)Hits / HitsMax;

            if (_stage == 1 && pct < 0.75) TransitionTo(2);
            else if (_stage == 2 && pct < 0.50) TransitionTo(3);
            else if (_stage == 3 && pct < 0.25) TransitionTo(4);
        }

        private void TransitionTo(int newStage)
        {
            _stage        = newStage;
            _inTransition = true;
            Frozen        = true;

            PublicOverheadMessage(MessageType.Emote, 0x3B2, false,
                "*Azura channels the raw power of the nexus!*");
            Effects.PlaySound(Location, Map, 0x2F3);

            // NEW: use EffectItem overload, drop EffectLayer
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3818, 16, 10, 9009, 0, 0, 0
            );

            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
            {
                Frozen        = false;
                _inTransition = false;
                PublicOverheadMessage(MessageType.Emote, 0x3B2, false,
                    $"*Azura unleashes Stage {_stage}!*");
            });
        }

        private void PerformStageAbility()
        {
            switch (_stage)
            {
                case 1: SummonMinions();        break;
                case 2: ArcaneLightningSpires();break;
                case 3: RunicCircleBurst();     break;
                case 4: NexusWhirlwind();       break;
            }
        }

        // ── Stage 1 ──
        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false,
                "*Azura tears open rifts to summon guardians!*");

            for (int i = 0; i < 3; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                Point3D spawn = new Point3D(Location.X + dx, Location.Y + dy, Location.Z);
                BaseCreature minion = new FireElemental();
                minion.MoveToWorld(spawn, Map);

                // NEW: use EffectItem overload
                Effects.SendLocationParticles(
                    EffectItem.Create(spawn, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, 2604, 0, 5029, 0
                );
                Effects.PlaySound(spawn, Map, 0x208);
            }
        }

        // ── Stage 2 ──
        private void ArcaneLightningSpires()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false,
                "*Crackling bolts of arcane energy rain down!*");

            for (int i = 0; i < 4; i++)
            {
                int dx = Utility.RandomMinMax(-5, 5);
                int dy = Utility.RandomMinMax(-5, 5);
                Point3D target = new Point3D(Location.X + dx, Location.Y + dy, Location.Z);

                // REPLACED: SendBoltEffect doesn’t have an overload with 8 args :contentReference[oaicite:1]{index=1}
                // → use the location‑based effect instead:
                Effects.SendLocationEffect(
                    target,      // where to strike
                    Map,
                    0x26F3,      // the bolt graphic
                    20,          // duration
                    0,           // speed (unused by LocationEffect)
                    1157,        // hue
                    0            // renderMode
                );

                Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
                {
                    Effects.PlaySound(target, Map, 0x5C5);
                    Effects.SendLocationParticles(
                        EffectItem.Create(target, Map, EffectItem.DefaultDuration),
                        0x3789, 12, 15, 1157, 0, 0, 0
                    );

                    foreach (Mobile m in Map.GetMobilesInRange(target, 1))
                        AOS.Damage(m, this, Utility.RandomMinMax(60, 100), 0, 0, 0, 0, 100);
                });
            }
        }

        // ── Stage 3 ──
        private void RunicCircleBurst()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false,
                "*Motes of eldritch energy form a circle of runes!*");

            var pts = new List<Point3D>();
            int radius = 4;
            for (int deg = 0; deg < 360; deg += 45)
            {
                double rad = Math.PI * deg / 180.0;
                int x = Location.X + (int)(radius * Math.Cos(rad));
                int y = Location.Y + (int)(radius * Math.Sin(rad));
                pts.Add(new Point3D(x, y, Location.Z));
            }

            foreach (var p in pts)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x382A, 20, 10, 1154, 0, 0, 0
                );
            }

            Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
            {
                foreach (var p in pts)
                {
                    Effects.PlaySound(p, Map, 0x307);
                    Effects.SendLocationParticles(
                        EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                        0x3779, 15, 12, 1154, 0, 0, 0
                    );
                    foreach (var m in Map.GetMobilesInRange(p, 0))
                        AOS.Damage(m, this, Utility.RandomMinMax(80, 120), 100, 0, 0, 0, 0);
                }
            });
        }

        // ── Stage 4 ──
        private void NexusWhirlwind()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false,
                "*The nexus tears reality apart in a deadly whirlwind!*");

            // Horizontal sweep
            for (int dx = -5; dx <= 5; dx++)
            {
                var p = new Point3D(Location.X + dx, Location.Y, Location.Z);
                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x3818, 8, 20, 1153, 0, 0, 0
                );
                Timer.DelayCall(TimeSpan.FromSeconds(0.4), () =>
                {
                    foreach (var m in Map.GetMobilesInRange(p, 0))
                        AOS.Damage(m, this, Utility.RandomMinMax(100, 150), 0, 100, 0, 0, 0);
                });
            }

            // Vertical sweep
            for (int dy = -5; dy <= 5; dy++)
            {
                var p = new Point3D(Location.X, Location.Y + dy, Location.Z);
                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x3818, 8, 20, 1153, 0, 0, 0
                );
                Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
                {
                    foreach (var m in Map.GetMobilesInRange(p, 0))
                        AOS.Damage(m, this, Utility.RandomMinMax(100, 150), 0, 100, 0, 0, 0);
                });
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            // add loot here
        }

        public NexusWeaver(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(_stage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
            _stage = reader.ReadInt();
        }
    }
}
