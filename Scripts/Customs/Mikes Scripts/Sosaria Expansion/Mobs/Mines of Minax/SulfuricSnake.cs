using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a sulfuric snake corpse")]
    public class SulfuricSnake : BaseCreature
    {
        // Ability cooldowns
        private DateTime m_NextAcidSpray;
        private DateTime m_NextMiasma;
        private DateTime m_NextEruption;
        private Point3D m_LastLoc;

        // A vivid toxic-green
        private const int UniqueHue = 0xB79;

        [Constructable]
        public SulfuricSnake()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.3, 0.6)
        {
            Name = "a sulfuric snake";
            Body = 52;                  // same body as lava snake
            Hue = UniqueHue;
            BaseSoundID = 0xDB;

            // —— Stats —— 
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(1000, 1200);
            SetStam(250, 300);
            SetMana(0);

            SetDamage(20, 30);

            // Mix of acid (poison) and fire
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Poison, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 60;
            ControlSlots = 4;

            // Loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new SprintersDilemma());

            // Initialize timers
            m_NextAcidSpray = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMiasma    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextEruption  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLoc = this.Location;
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;
        
        public SulfuricSnake(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Combatant == null)
                return;

            // — Leave behind poison trail when moving —
            if (Location != m_LastLoc && Utility.RandomDouble() < 0.2)
            {
                var tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(m_LastLoc, Map);
            }
            m_LastLoc = Location;

            // Cast abilities when off cooldown
            if (DateTime.UtcNow >= m_NextAcidSpray && InRange(Combatant.Location, 12))
            {
                AcidicSpray();
                m_NextAcidSpray = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            if (DateTime.UtcNow >= m_NextMiasma)
            {
                SulfuricMiasma();
                m_NextMiasma = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            if (DateTime.UtcNow >= m_NextEruption && InRange(Combatant.Location, 2))
            {
                MagmaEruption();
                m_NextEruption = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
        }

        // —— Ability #1: Acidic Spray — ranged acid blast + armor corrosion
        private void AcidicSpray()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Hissssss!*");
                PlaySound(0x227);
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, Map),
                    new Entity(Serial.Zero, target.Location, Map),
                    0x36A4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                DoHarmful(target);
                int damage = Utility.RandomMinMax(40, 60);
                AOS.Damage(target, this, damage, 0, 0, 0, 40, 60); // 40% fire, 60% poison

                // Apply armor corrosion (temporary lower physical resist)
                int reduce = Utility.RandomMinMax(5, 10);
                target.SendLocalizedMessage(1070837, "", 0x22); // "Your armor corrodes!"
                Timer.DelayCall(TimeSpan.Zero, () =>
                {
                    if (target.Alive)
                        target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                });
                target.FixedEffect(0x374A, 10, 15);
            }
        }

        // —— Ability #2: Sulfuric Miasma — poison gas cloud around self
        private void SulfuricMiasma()
        {
            Say("*You cannot escape the fumes!*");
            PlaySound(0x22D);
            for (int i = 0; i < 6; i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);
            }
        }

        // —— Ability #3: Magma Eruption — lava pools burst out around itself
        private void MagmaEruption()
        {
            Say("*ROOOAAAR!*");
            PlaySound(0x208);
            var around = new List<Point3D>();
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
                around.Add(new Point3D(X + dx, Y + dy, Z));

            foreach (var loc in around)
            {
                Point3D p = loc;
                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = Map.GetAverageZ(p.X, p.Y);

                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(p, Map);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();

            // Re-init timers
            m_NextAcidSpray = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMiasma    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextEruption  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
