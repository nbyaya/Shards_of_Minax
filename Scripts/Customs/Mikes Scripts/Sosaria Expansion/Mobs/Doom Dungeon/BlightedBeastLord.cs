using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a blighted beast lord corpse")]
    public class BlightedBeastLord : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime _nextAuraTime;
        private DateTime _nextDischargeTime;
        private DateTime _nextCloudTime;
        private DateTime _nextSummonTime;
        private Point3D  _lastLocation;

        // A sickly green glow to mark its corruption
        private const int UniqueHue = 1271;

        [Constructable]
        public BlightedBeastLord()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "a Blighted Beast Lord";
            Body           = 775;        // same body as PlagueBeastLord
            BaseSoundID    = 679;        // same base sounds
            Hue            = UniqueHue;
            SpeechHue      = 0x3B2;

            // Attributes
            SetStr(650, 750);
            SetDex(150, 200);
            SetInt(200, 250);

            SetHits(3000, 3500);
            SetStam(200, 250);
            SetMana(200, 300);

            SetDamage(30, 40);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison,   60);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   90, 100);
            SetResistance(ResistanceType.Energy,   40, 50);

            // Skills
            SetSkill(SkillName.Tactics,       120.0, 130.0);
            SetSkill(SkillName.Wrestling,     120.0, 130.0);
            SetSkill(SkillName.MagicResist,   120.0, 130.0);

            Fame       = 30000;
            Karma      = -30000;
            VirtualArmor = 100;
            ControlSlots = 6;

            // Initialize ability timers
            var now = DateTime.UtcNow;
            _nextAuraTime      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5));
            _nextDischargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _nextCloudTime     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextSummonTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            _lastLocation = this.Location;

            // Basic loot
            PackItem(new GreaterPoisonPotion());
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackGold(500, 1000);
        }

        // Corrupting Aura: pulses poison on anyone stepping too close
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (DateTime.UtcNow < _nextAuraTime || !Alive || m == null || m == this)
                return;

            if (m is Mobile target && target.Alive && target.Map == this.Map && target.InRange(this.Location, 3) && CanBeHarmful(target, false))
            {
                _nextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);

                DoHarmful(target);
                target.ApplyPoison(this, Poison.Greater);
                target.SendMessage(0x22, "The Blighted Beast Lord's aura burns with foul decay!");
                target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                target.PlaySound(0x1F8);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Summon blighted minions
            if (now >= _nextSummonTime)
            {
                SummonMinions();
                _nextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            // Necrotic discharge AoE
            else if (now >= _nextDischargeTime)
            {
                NecroticDischargeAttack();
                _nextDischargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Toxic cloud on target
            else if (now >= _nextCloudTime && Combatant is Mobile targetCloud && InRange(targetCloud.Location, 10))
            {
                ToxicCloudAttack(targetCloud);
                _nextCloudTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        private void NecroticDischargeAttack()
        {
            if (Map == null) return;

            this.PlaySound(0x1C1);
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                int damage = Utility.RandomMinMax(50, 80);
                AOS.Damage(t, this, damage, 0, 0, 0, 0, 100);
                t.ApplyPoison(this, Poison.Deadly);
                t.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        private void ToxicCloudAttack(Mobile target)
        {
            if (Map == null || target == null) return;

            this.Say("*Breathe the blight!*");
            this.PlaySound(0x1C0);

            var loc = target.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            // Spawn a lingering toxic gas tile
            var cloud = new PoisonTile();
            cloud.Hue = UniqueHue;
            cloud.MoveToWorld(loc, Map);
        }

        private void SummonMinions()
        {
            if (Map == null) return;

            this.Say("*Rise, my cursed brood!*");
            this.PlaySound(0x1BF);

            for (int i = 0; i < 3; i++)
            {
                // Assume PlagueBeastMinion is defined elsewhere
                var spawn = new Zombie();
                spawn.Hue = UniqueHue;

                var xOff = Utility.RandomMinMax(-4, 4);
                var yOff = Utility.RandomMinMax(-4, 4);
                var pt   = new Point3D(X + xOff, Y + yOff, Z);

                if (!Map.CanFit(pt, 16, false, false))
                    pt.Z = Map.GetAverageZ(pt.X, pt.Y);

                spawn.MoveToWorld(pt, Map);
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, 1071920); // * The blight spreads as the Beast Lord falls! *
                Effects.PlaySound(Location, Map, 0x1C2);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Spawn hazardous tiles around the corpse
                int count = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < count; i++)
                {
                    int xOff = Utility.RandomMinMax(-3, 3);
                    int yOff = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + xOff, Y + yOff, Z);

                    if (!Map.CanFit(loc, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    // Alternate between PoisonTile and ToxicGasTile
                    var tile = (i % 2 == 0) ? (Item)new PoisonTile() : new ToxicGasTile();
                    tile.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // Sound overrides
        public override int GetIdleSound()   => 0x1BF;
        public override int GetAttackSound() => 0x1C0;
        public override int GetHurtSound()   => 0x1C1;
        public override int GetDeathSound()  => 0x1C2;

        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 130.0;
        public override double DispelFocus      => 65.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new DancersOath()); // Unique artifact

            if (Utility.RandomDouble() < 0.10)
                PackItem(new VoidCore());
        }

        public BlightedBeastLord(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers after load
            var now = DateTime.UtcNow;
            _nextAuraTime      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5));
            _nextDischargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _nextCloudTime     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextSummonTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }

        // Carving and scissoring use the same logic as PlagueBeastLord (not shown for brevity)
    }
}
