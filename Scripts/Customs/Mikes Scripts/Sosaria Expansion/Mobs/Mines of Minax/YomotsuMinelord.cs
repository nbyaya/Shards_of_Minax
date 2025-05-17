using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a yomotsu minelord corpse")]
    public class YomotsuMinelord : BaseCreature
    {
        private static readonly int UniqueHue = 1204; // Deep emerald
        private DateTime _nextShockwave, _nextMinefield, _nextStomp;
        private Point3D _lastDropLocation;

        [Constructable]
        public YomotsuMinelord() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Yomotsu Minelord";
            Body = 245;
            Hue = UniqueHue;
            BaseSoundID = 0x452;

            // —— Stats ——
            SetStr(600, 700);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(1000, 1200);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(15, 25);

            // —— Damage types ——
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Poison, 20);

            // —— Resistances ——
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            // —— Skills ——
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 75;
            ControlSlots = 5;

            // —— Ability cooldowns ——
            _nextShockwave = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextMinefield = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            _nextStomp     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            _lastDropLocation = this.Location;

            // —— Loot basics ——
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));
            PackItem(new GreenGourd());
            PackGold(2000, 3500);
        }

        // —— Poison on hit —— 
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.25 > Utility.RandomDouble() && defender is Mobile target)
            {
                // Apply stronger poison
                target.SendMessage("The Minelord's venomous claws tear into you!");
                target.ApplyPoison(this, Poison.Deadly);
            }
        }

        // —— Thin trail of landmines as it moves —— 
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Drop a landmine tile at its previous location, 10% chance
            if (this.Location != _lastDropLocation && Utility.RandomDouble() < 0.10)
            {
                var loc = _lastDropLocation;
                _lastDropLocation = this.Location;

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(loc, Map);
                }
            }
            else
            {
                _lastDropLocation = this.Location;
            }
        }

        // —— Main AI loop —— 
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // 1) Shockwave: AoE quake around self
            if (now >= _nextShockwave)
            {
                Shockwave();
                _nextShockwave = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                return;
            }

            // 2) Minefield: scatter multiple landmines around target
            if (now >= _nextMinefield && Combatant is Mobile mineTarget)
            {
                Minefield(mineTarget);
                _nextMinefield = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                return;
            }

            // 3) Stomp: focused quicksand trap under the combatant
            if (now >= _nextStomp && Combatant is Mobile stompTarget && InRange(stompTarget, 6))
            {
                QuicksandStomp(stompTarget);
                _nextStomp = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
        }

        private void Shockwave()
        {
            PlaySound(0x2F1);
            FixedParticles(0x373A, 1, 30, 5030, UniqueHue, 0, EffectLayer.CenterFeet);

            // Damage all nearby
            foreach (var o in GetMobilesInRange( 5 ))
            {
                if (o != this && CanBeHarmful(o, false) && o is Mobile m)
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 100, 0, 0, 0, 0);
                    m.SendMessage("The earth shatters beneath your feet!");
                }
            }
        }

        private void Minefield(Mobile target)
        {
            Say("*The ground caves in!*");
            PlaySound(0x22F);

            // Scatter 6–8 mines around the target
            for (int i = 0; i < Utility.RandomMinMax(6, 8); i++)
            {
                int dx = Utility.RandomMinMax(-2, 2);
                int dy = Utility.RandomMinMax(-2, 2);
                var loc = new Point3D(target.X + dx, target.Y + dy, target.Z);

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(loc, Map);
                }
            }
        }

        private void QuicksandStomp(Mobile target)
        {
            Say("*Feel the weight of the earth!*");
            PlaySound(0x212);

            // Warning effect
            Effects.SendTargetParticles(target, 0x3728, 10, 15, UniqueHue, 0, 5039, 0, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (target.Alive && CanBeHarmful(target, false))
                {
                    var loc = target.Location;
                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var quicksand = new QuicksandTile();
                        quicksand.Hue = UniqueHue;
                        quicksand.MoveToWorld(loc, Map);
                    }
                }
            });
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, 3);

            if (Utility.RandomDouble() < 0.05) // 5% for a special pickaxe
                PackItem(new VerdigrisPanels());
        }

        // —— Standard overrides —— 
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;
        public override int GetIdleSound()  => 0x42A;
        public override int GetAttackSound()=> 0x435;
        public override int GetHurtSound()  => 0x436;
        public override int GetDeathSound() => 0x43A;

        public YomotsuMinelord(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‐initialize timers
            _nextShockwave  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextMinefield  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            _nextStomp      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _lastDropLocation = this.Location;
        }
    }
}
